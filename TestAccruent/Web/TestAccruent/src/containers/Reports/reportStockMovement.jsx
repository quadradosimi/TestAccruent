import React, { useEffect, useState } from "react";
import axios from "axios";
import { Link } from "react-router-dom";
import 'bootstrap/dist/css/bootstrap.css';
import 'font-awesome/css/font-awesome.min.css';
import configData from "../../config.json";

const reportStockMovement = () => {

  const root = configData.SERVER_URL;
  const reportStockMovementApi = root + "report";
  const populateProductComboApi = root + "product";
  const getJWTTokenApi = root + "auth/loginJwt";
  var token = configData.TOKEN;;
  
  const [stock, setStock] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [isLoadingProduct, setIsLoadingProduct] = useState(false);
  const [error, setError] = useState(null);

  const [products, setProducts] = useState([]);

  const [filter, setFilter] = useState({
      movementDate: "",
      productId: ""
  })
  
  useEffect(() => {
    getreportStockMovement();
  }, []);

  const getreportStockMovement = () => {
    //if the token dont exist yet
    if(token == "")
    {
      getJwtToken();
    }
    else{
      setIsLoading(true);
      axios
        .get(reportStockMovementApi.concat("/") + '1', { headers: {"Authorization" : `Bearer ${token}`} })
        .then((res) => {
          if(isLoadingProduct == false)
          {            
            populateProductCombo();
          }
          setStock(res.data);        
          setIsLoading(false);
        })
        .catch((error) => {
          document.getElementById('errorMessage').innerHTML = error.message;
        });
    }
  };

  const populateProductCombo = () => {
    setIsLoadingProduct(true);

    axios
    .get(populateProductComboApi, { headers: {"Authorization" : `Bearer ${token}`} })
    .then((res) => {
      setProducts(res.data);
      setIsLoadingProduct(false);
    })
  };

  const handelSearch = async () => {
    document.getElementById('errorMessage').innerHTML = '';

    if(document.getElementById('movementDate').value != '')
    {
      setIsLoading(true);

      // get token from storage
      token = localStorage.getItem("tokenAccruent");
  
      let data = filter.movementDate + ';' +  filter.productId;
  
      axios
      .get(reportStockMovementApi.concat("/") + data, { headers: {"Authorization" : `Bearer ${token}`} })
        .then((item) => {
          setStock(item.data);
          setIsLoading(false);
        })
        .catch((error) => {
          document.getElementById('errorMessage').innerHTML = error.message;
        });
    }
    else
    {
      document.getElementById('errorMessage').innerHTML = 'Preencher a data é obrigatório para fazer busca.'
    }

  };

  const handelInput = (event) => {
    event.preventDefault();
    const { name, value } = event.target;
    console.log(name, value)
    setFilter({ ...filter, [name]: value });

    //erase error message from filter
    if(document.getElementById("movementDate").value != "")
    {
      document.getElementById('errorMessage').innerHTML = '';
    }
}

const getJwtToken = async () => {
  setIsLoading(true);
    try {
      setIsLoading(true);
      const response = await fetch(getJWTTokenApi, {
          method: 'POST',
          headers: {
              'Content-Type': 'application/json'
          },
          body:JSON.stringify({
            Username: "admin",
            Password: "password"
          }),
      });

      if (response.ok) {    
        const json = await response.json();
        //get token from api
        token = json.token;
        //remove previous data from storage
        localStorage.removeItem("tokenAccruent");
        //save token
        localStorage.setItem("tokenAccruent", token);

        getreportStockMovement();
        
      } else {
          document.getElementById('errorMessage').innerHTML = 'Erro ao salvar o form.';
      }

  } catch (error) {
      document.getElementById('errorMessage').innerHTML = error.message;
  } finally{
      setIsLoading(false);
  }
};
  return (
      <div className="container">
        <div className="row">
          <nav className="navbar navbar-expand-sm navbar-dark bg-dark">
            <div className="container-fluid">
              <div className="navbar-brand" href="#">
                <span className="navbar-text">Relatório de estoque</span>
              </div>
              <button
                className="navbar-toggler"
                type="button"
                data-bs-toggle="collapse"
                data-bs-target="#mynavbar"
              >
                <span className="navbar-toggler-icon"></span>
              </button>
              <div className="collapse navbar-collapse" id="mynavbar">
                <ul className="navbar-nav ms-auto">
                  <li className="nav-item">
                    <Link className="nav-link" to="/create-stock">
                      Nova Movimentação
                    </Link>
                  </li>
                </ul>
              </div>
            </div>
          </nav>
        </div>
        <div className="row">
          <nav className="navbar navbar-expand-sm navbar-dark bg-dark">
          <div className="container-fluid">
            <div className="row">
              <div className='col-6'>
                  <select className="form-control" id="productId" name="productId" onChange={handelInput}>
                      <option value="">Selecione uma opção</option> 
                      {products.map(product => (<option key={product.name} value={product.id}>{product.name}</option>))}
                  </select>
                </div>
                <div className='col-5'>
                <input type="date" className="form-control" id="movementDate" name="movementDate" onChange={handelInput} />
                </div>
                <div className='col-1'>
                  <button type="submit" id="btnSearch" className="btn btn-primary submit-btn" onClick={() => handelSearch()} >
                    <i className="fa fa-search"
                      aria-hidden="true"                        
                    ></i>
                </button>
              </div>                  
            </div>
            </div>
          </nav>
        </div>
        <div className="row">
          <table className="table table-striped">
            <thead>
              <tr>
                <th>Nome do Produto</th>
                <th>Código do produto</th>
                <th>Entradas</th>
                <th>Saídas</th>
                <th>Saldo</th>
              </tr>
            </thead>
            <tbody>
              {stock?.map((item, i) => {
                return (
                  <tr key={i + 1}>
                    <td>{item.productName}</td>
                    <td>{item.productCode}</td>
                    <td>{item.totalCredits}</td>
                    <td>{item.totalDebits}</td>
                    <td>{item.totalBalance}</td>
                  </tr>
                );
              })}
            </tbody>
          </table>
        </div>
        <div className="row mb-4">
            <div className="text-danger text-center font-weight-bold" id="errorMessage"></div>
        </div>
    </div>
  )
}

export default reportStockMovement;