import React, { useEffect, useState, useContext } from "react";
import axios from "axios";
import InfoCard from "./InfoCard"; // Presupunând că ai o componentă generică de card numită InfoCard
import { useUser } from "../../../../context/LoginRequired"; // Presupunând că există un context pentru autentificare

const ReturnedOverdueBooksCard = () => {
    const [booksCount, setBooksCount] = useState(null);
    const  user  = useUser(); // Acces la user și rol din contextul de autentificare

    useEffect(() => {
        const fetchBooksCount = async () => {
            let url = 'https://localhost:7115/api/v1/Books/';
            let titleText = '';

            if (user.role === 'Admin') {
                url += 'GetReturnedOverdueBooks';
                titleText = 'Returned Overdue Books';
            } else {
                url += `getoverduebooksbyuserid?userId=${user.userId}`;
                titleText = "Books Overdue By You";
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
    }, [user]); // Dependență de user pentru reîncărcare la schimbarea utilizatorului

    if (booksCount === null) {
        return <div>Loading...</div>;
    }

    return (
        <InfoCard
            title={user.role === 'Admin' ? 'Returned Overdue Books' : "You're Overdues"}
            value={booksCount}
        />
    );
};

export default ReturnedOverdueBooksCard;
