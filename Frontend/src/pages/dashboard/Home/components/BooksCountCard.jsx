import React, { useEffect, useState, useContext } from "react";
import axios from "axios";
import InfoCard from "./InfoCard";
import { useUser } from "../../../../context/LoginRequired"; // Asigură-te că acesta este importul corect

const BooksCountCard = () => {
    const [booksCount, setBooksCount] = useState(null);
    const user = useUser(); // Utilizează contextul pentru a accesa informațiile utilizatorului

    useEffect(() => {
        const fetchBooksCount = async () => {
            let url = 'https://localhost:7115/api/v1/Books/';

            // Decide care endpoint să folosească în funcție de rolul utilizatorului
            if (user.role === 'Admin') {
                // Pentru admin, afișează toate cărțile din baza de date
                url += 'GetAllBooks';
            } else {
                // Pentru user obișnuit, afișează doar cărțile disponibile
                url += 'GetAvailableBooks';
            }

            try {
                const response = await axios.get(url, {
                    headers: { Authorization: `Bearer ${user.token}` }
                });

                if (response.status === 200) {
                    setBooksCount(response.data.length);
                } else {
                    console.error('Failed to fetch books count: ', response.status);
                }
            } catch (error) {
                console.error('Error fetching books count: ', error);
            }
        };

        fetchBooksCount();
    }, [user]);

    if (booksCount === null) {
        return <div>Loading...</div>;
    }

    return (
        <InfoCard
            title={user.role === 'Admin' ? 'Total Books in Library' : 'Available Books'}
            value={booksCount}
        />
    );
};

export default BooksCountCard;
