import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { Route, Routes } from "react-router-dom";
import { BrowserRouter } from 'react-router-dom';
import ReportStockMovement from './containers/Reports/reportStockMovement'
import CreateStockMovement from './containers/Register/addStockMovement'
import AuthProvider from "./hooks/AuthProvider";


createRoot(document.getElementById('root')).render(
  <StrictMode>
    <BrowserRouter>
      <AuthProvider>
        <Routes>          
          <Route path="/" element={<ReportStockMovement/>} />
           <Route path="/create-stock" element={<CreateStockMovement/>} /> 
        </Routes>
      </AuthProvider>     
    </BrowserRouter>
  </StrictMode>
)
