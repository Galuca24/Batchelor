import React, { useState } from 'react';
import { Navigate } from "react-router-dom";
import {  Stack } from '@mui/material';
import LoansTable from './components/LoansTable';
import FavoriteBooksTable from './components/FavoriteBooks';
import FinesTable from './components/FinesTable';
import { useUser } from '../../../context/LoginRequired';
import { Button } from "@material-tailwind/react";
import FavoriteBooks from './components/FavoriteBooks';

const UserLibraryPage = () => {
  const [currentView, setCurrentView] = useState("Loans");
  const user = useUser();

  // if (user.role !== "User") {
  //   return <Navigate to="/dashboard/home" />;
  // }
  const handleChangeView = (view) => {
    setCurrentView(view);
    localStorage.setItem('currentView', view);  // Salvează starea în localStorage
  };
  return (
    <div>
      <Stack direction="row" spacing={2} sx={{ mb: 2 }}>
        
      <Button
        onClick={() => handleChangeView('Favorites')}
        className={`font-medium w-full sm:w-1/2 md:w-1/4 lg:w-1/5 ${currentView === "Favorites" ? 'bg-surface-light-green text-white' : 'bg-surface-dark-green text-surface-white'} hover:bg-surface-dark-green hover:text-white`}
        >
          Favorite Books
        </Button>
        
        <Button
        onClick={() => handleChangeView('Loans')}
        className={`font-medium w-full sm:w-1/2 md:w-1/4 lg:w-1/5 ${currentView === "Loans" ? 'bg-surface-light-green text-white' : 'bg-surface-dark-green text-surface-white'} hover:bg-surface-dark-green hover:text-white`}
        >
          My Loans
        </Button>

        <Button
        onClick={() => handleChangeView('Fines')}
          className={`font-medium w-full sm:w-1/2 md:w-1/4 lg:w-1/5 ${currentView === "Fines" ? 'bg-surface-light-green text-white' : 'bg-surface-dark-green text-surface-white'} hover:bg-surface-dark-green hover:text-white`}
        >
          My Fines
        </Button>
      </Stack>
      {/* Content pentru view-ul selectat */}
      {currentView === 'Loans' && <LoansTable />}
      {currentView === 'Favorites' && <FavoriteBooks/>}
      {currentView === 'Fines' && <FinesTable/>}
    </div>
  );
};

export default UserLibraryPage;
