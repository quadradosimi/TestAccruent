import React, { useEffect, useState } from 'react'
import {useNavigate } from "react-router-dom";
import { Link } from "react-router-dom";
import 'bootstrap/dist/css/bootstrap.css';
import 'font-awesome/css/font-awesome.min.css';
import configData from "../../config.json";
import axios from "axios";

const CreateStockMovement = () => {

    const navigate = useNavigate();
    const root = configData.SERVER_URL;
    const createStockMovemenApi = root + "stock";
    const validateNegativeStockApi = root + "stock/validate";
    const validateIfProductExistApi = root + "product/validate";
    const populateProductComboApi = root + "product";
    const token = localStorage.getItem("tokenAccruent");

    const [error, setError] = useState(null);
    const [isLoading, setIsLoading] = useState(false);
    const [stock, setStock] = useState({
    })

    const [products, setProducts] = useState([]);
    
    useEffect(() => {
        populateProductCombo();
      }, []);      
    
    const populateProductCombo = () => {
        axios
        .get(populateProductComboApi, { headers: {"Authorization" : `Bearer ${token}`} })
        .then((res) => {
            setProducts(res.data);
        })
        .catch((err) => {
        document.getElementById('errorMessage').innerHTML = err.message;
        });
    };

    async function saveStock(){
        try {
            setIsLoading(true);
            const response = await fetch(createStockMovemenApi, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization' : `Bearer ${token}`
                },
                body: JSON.stringify(stock),
            });

            if (response.ok) {     
                navigate('/');
            } else {
                document.getElementById('errorMessage').innerHTML = 'Erro ao salvar o form.';
            }

        } catch (error) {
            document.getElementById('errorMessage').innerHTML = error.message;
        } finally{
            setIsLoading(false);
        }
    }

    async function validateNegativeStock(){
        setIsLoading(true);

        //set data
        let data = stock.productId + ';' +  stock.quantity;

        axios
        .get(validateNegativeStockApi.concat("/") + data, { headers: {"Authorization" : `Bearer ${token}`} })
        .then((item) => {
            if(item.data)
            { 
                saveStock();
            }
            else
            {
                document.getElementById('errorMessage').innerHTML = 'Movimentação inválida. Total estoque ficará negativo.';
            }
            setIsLoading(false);
        })
        .catch((error) => {
            document.getElementById('errorMessage').innerHTML = err.message;
        });
    }

    async function validateIfProductExist(){
        setIsLoading(true);
        let data = stock.productId;

        axios
        .get(validateIfProductExistApi.concat("/") + data, { headers: {"Authorization" : `Bearer ${token}`} })
        .then((item) => {
            if(item.data)
            { 
                if(stock.type == "saida")     
                {
                    validateNegativeStock();
                }
                else
                {
                    saveStock();
                }
            }
            else
            {
                document.getElementById('errorMessage').innerHTML = 'O produto escolhido não existe na base de dados.';
                return false;
            }

            setIsLoading(false);
        })
        .catch((error) => {
            document.getElementById('errorMessage').innerHTML = error.message;
        });
    }

    async function validations(){

        validateIfProductExist();
    }

    const handelInput = (event) => {
        event.preventDefault();
        const { name, value } = event.target;
        console.log(name, value)
        setStock({ ...stock, [name]: value });
    }

    const handelSubmit = async () => {
        event.preventDefault(); 

        document.getElementById('errorMessage').innerHTML = '';

        let validation = true;
        let message = '';
        
        //validation before save
        validations();
 }

    return (
        <div className="container">
            <div className="row">
                <div className='stock-form'>
                    <div className='heading fw-bold'>
                        <p>Adicionar Movimentações no Estoque</p>
                    </div>
                    <form onSubmit={handelSubmit}>
                        <div className="mb-3">
                            <label htmlFor="productId" className="form-label">Produto</label>
                                <select className="form-control" id="productId" name="productId" onChange={handelInput} required>
                                    <option value="">Selecione uma opção</option> 
                                    {products.map(product => (<option key={product.name} value={product.id}>{product.name}</option>))}
                                </select>
                        </div>
                        <div className="mb-3">
                            <label htmlFor="type" className="form-label">Tipo</label>
                            <select className="form-control" id="type" name="type" value={stock.tipo} onChange={handelInput} required>
                                <option value="">Selecione uma opção</option>
                                <option value="entrada">Entrada</option>
                                <option value="saida">Saída</option>
                                
                            </select>
                        </div>
                        <div className="mb-3">
                            <label htmlFor="quantity" className="form-label">Quantidade</label>
                            <input type="number" className="form-control" id="quantity" name="quantity" value={stock.quantity} onChange={handelInput} required />
                        </div>

                        <button type="submit" className="btn btn-primary submit-btn">Salvar</button>

                        <div className="row mb-4">
                            <div className="text-danger text-center font-weight-bold" id="errorMessage"></div>
                        </div>
                        
                        <div className="mt-2">
                            <Link to={`/`}>
                                Voltar
                            </Link>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    )
}

export default CreateStockMovement