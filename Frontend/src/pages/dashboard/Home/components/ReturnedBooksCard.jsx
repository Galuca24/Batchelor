import React, { useEffect, useState } from "react";
import axios from "axios";
import InfoCard from "./InfoCard"; // Presupunem că ai deja o componentă InfoCard
import { useUser } from "../../../../context/LoginRequired";

const ReturnedBooksCard = () => {
    const [returnedBooksCount, setReturnedBooksCount] = useState(null);
    const user = useUser();

    useEffect(() => {
        const fetchReturnedBooksCount = async () => {
            let url;
            if (user.role === 'Admin') {
                url = 'https://localhost:7115/api/v1/Books/ReturnedBooksCount';
            } else {
                url = `https://localhost:7115/api/v1/Books/GetReturnedBooksByUserCount/${user.userId}`;
            }

            try {
                const response = await axios.get(url, {
                    headers: { Authorization: `Bearer ${user.token}` }
                });

                if (response.status === 200) {
                    setReturnedBooksCount(response.data);
                } else {
                    console.error('Failed to fetch returned books count: ', response.status);
                }
            } catch (error) {
                console.error('Error fetching returned books count: ', error);
            }
        };

        fetchReturnedBooksCount();
    }, [user]);

    // Dacă încă se încarcă datele, arată un indicator de încărcare
    if (returnedBooksCount === null) {
        return <div>Loading...</div>;
    }

    return (
        <InfoCard
            title={user.role === 'Admin' ? 'Returned Books' : 'Your Returned Books'}
            value={returnedBooksCount}
            
        />
    );
};

export default ReturnedBooksCard;
