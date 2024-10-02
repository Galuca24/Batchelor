import React from 'react';
import { Navigate } from 'react-router-dom';
import { useUser } from './LoginRequired';

const RequireRole = ({ children, requiredRole }) => {
    const { role } = useUser();

    if (!role) {
        // Dacă rolul nu este încă încărcat, puteți decide să arătați un spinner sau alt indicator de încărcare
        return <div>Loading...</div>;
    }

    if (role !== requiredRole) {
        // Dacă utilizatorul nu are rolul necesar, redirecționați către pagina principală sau de login
        return <Navigate to="/dashboard/home" replace />;
    }

    return children;
};

export default RequireRole;