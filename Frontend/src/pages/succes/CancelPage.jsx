import { useLocation } from 'react-router-dom';
import queryString from 'query-string';
import React, { useEffect } from 'react';
import axios from 'axios';
import { useNavigate } from "react-router-dom";
import { toast } from 'react-toastify';

function CancelPage() {
  const location = useLocation();
  const { fineId } = queryString.parse(location.search);
  const navigate = useNavigate();

  useEffect(() => {
    setTimeout(() => {
      navigate('/dashboard/home');
    }, 5000);
  }, [navigate]);

  const containerStyles = {
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
    justifyContent: 'center',
    minHeight: '100vh', 
    textAlign: 'center'
  };

  const imageStyles = {
    maxHeight: '65vh', 
    width: 'auto', 
    marginTop: '20px' 
  };

  const headerStyles = {
    fontSize: '2.2rem',
    color: '#FF0000', 
    backgroundColor: 'rgba(255, 255, 255, 0.8)',
    padding: '10px',
    borderRadius: '10px',
    boxShadow: '0 4px 8px rgba(0, 0, 0, 0.1)'
  };

  return (
    <div style={containerStyles}>
      <h1 style={headerStyles}>Your payment process has been cancelled and you will be redirected shortly.</h1>
      <img src="/img/cancelBook.PNG" alt="Successful Payment" style={imageStyles}/>
    </div>
  );
}

export default CancelPage;
