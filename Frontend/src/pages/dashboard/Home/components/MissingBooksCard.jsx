import React, { useEffect, useState } from "react";
import axios from "axios";
import InfoCard from "./InfoCard"; // Presupunând că ai o componentă generică de card numită InfoCard

const MissingBooksCard = () => {
    const [missingBooksCount, setMissingBooksCount] = useState(null);

    useEffect(() => {
        const fetchMissingBooks = async () => {
            try {
                const response = await axios.get('https://localhost:7115/api/v1/Books/GetMissingBooks');
                if (response.status === 200) {
                    // Setează numărul de cărți lipsă
                    setMissingBooksCount(response.data.length);
                } else {
                    console.error('Failed to fetch missing books: ', response.status);
                }
            } catch (error) {
                console.error('Error fetching missing books: ', error);
            }
        };

        fetchMissingBooks();
    }, []);

    if (missingBooksCount === null) {
        return <div>Loading...</div>; 
    }

    return (
        <InfoCard
            title="Missing Books"
            value={missingBooksCount}
        />
    );
};

export default MissingBooksCard;
