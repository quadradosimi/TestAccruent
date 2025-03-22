import { useContext, createContext, useState } from "react";
import { useNavigate } from "react-router-dom";
import configData from "../config.json";

const AuthContext = createContext();

const AuthProvider = ({ children }) => {
const [user, setUser] = useState(null);
const [token, setToken] = useState(localStorage.getItem("site") || "");
const navigate = useNavigate();
const root = configData.SERVER_URL;
const authUserApi = root + "Auth/login";

const loginAction = async (data) => {
  try {
    const response = await fetch(authUserApi, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data),
    });

    const res = await response.json();
    if (res.ok) {
            setUser(res.data.email);
            setToken(res.data.id);
            localStorage.removeItem("site");
            localStorage.setItem("site", res.data.managerName);
            navigate("/register");
            return;
    } else {
        document.getElementById('errorMessage').innerHTML = 'Usuario e senha estão errados!';
        
    }

  } catch (error) {
    document.getElementById('errorMessage').innerHTML = error.message;
  } 
  };

const logOut = () => {
  setUser(null);
  setToken("");
  localStorage.removeItem("site");
  navigate("/login");
};

return (
  <AuthContext.Provider value={{ token, user, loginAction, logOut }}>
    {children}
  </AuthContext.Provider>
);

};

export default AuthProvider;

export const useAuth = () => {
return useContext(AuthContext);
};