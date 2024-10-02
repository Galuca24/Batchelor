import React, { useEffect, useState } from "react";
import axios from "axios";
import { useUser } from "../../../../context/LoginRequired";
import InfoCard from "./InfoCard"; 

const BorrowedBooksCard = () => {
    const [booksCount, setBooksCount] = useState(0);
    const user = useUser();

    useEffect(() => {
        const fetchBooksCount = async () => {
            let url = 'https://localhost:7115/api/v1/Books/';
            if (user.role === 'Admin') {
                url += 'GetLoanedBooksCount';
            } else if (user.role === 'User') {
                url += `GetActiveLoansByUser/${user.userId}`;
            }

            try {
                const response = await axios.get(url, {
                    headers: { Authorization: `Bearer ${user.token}` }
                });

                if (response.status === 200) {
                    // Admin receives a single number, User receives an array of objects
                    setBooksCount(user.role === 'Admin' ? response.data : response.data.length);
                } else {
                    console.error('Failed to fetch data: ', response.status);
                }
            } catch (error) {
                console.error('Error fetching data: ', error);
            }
        };

        if (user && user.token) {
            fetchBooksCount();
        }
    }, [user]);

    return (
        <InfoCard
            
            title={user.role === 'Admin' ? 'Borrowed Books' : 'Your Borrowed Books'}
            value={booksCount}
            colorClass="text-teal-600" 
        />
    );
};

export default BorrowedBooksCard;
