import { useLocation } from 'react-router-dom';
import queryString from 'query-string';
import React, { useEffect } from 'react';
import axios from 'axios';
import { useNavigate } from "react-router-dom";
import { toast } from 'react-toastify';

function SuccessPage() {
  const location = useLocation();
  const { fineId, userId } = queryString.parse(location.search);
  const navigate = useNavigate();

  useEffect(() => {
    const updatePaymentStatus = async () => {
      if (fineId) {
        try {
          await axios.put(`https://localhost:7115/api/v1/fines/payfine/${fineId}`);
        } catch (error) {
          console.error('Error updating payment status:', error);
          toast.error("Failed to update payment status. Please check the logs for more details.");
        }
      }
    };

    const activateMembership = async () => {
      if (userId) {
        try {
          await axios.post('https://localhost:7115/api/v1/memberships/activatemembership', { userId });
        } catch (error) {
          console.error('Error activating membership:', error);
          toast.error("Failed to activate membership. Please check the logs for more details.");
        }
      }
    };

    updatePaymentStatus();
    activateMembership();

    setTimeout(() => {
      navigate('/dashboard/home');
    }, 5000);
  }, [fineId, userId, navigate]);

  const containerStyles = {
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
    justifyContent: 'center',
    minHeight: '100vh', // This ensures that the whole viewport is covered
    textAlign: 'center'
  };

  const imageStyles = {
    maxHeight: '65vh', 
    width: 'auto', 
    marginTop: '20px' 
  };

  const headerStyles = {
    fontSize: '2.2rem',
    color: '#4CAF50', // Green text
    backgroundColor: 'rgba(255, 255, 255, 0.8)',
    padding: '10px',
    borderRadius: '10px',
    boxShadow: '0 4px 8px rgba(0, 0, 0, 0.1)'
  };

  return (
    <div style={containerStyles}>
      <h1 style={headerStyles}>Payment was successful! Your transaction has been processed.</h1>
      <img src="/img/happyBook.PNG" alt="Successful Payment" style={imageStyles}/>
    </div>
  );
}

export default SuccessPage;
