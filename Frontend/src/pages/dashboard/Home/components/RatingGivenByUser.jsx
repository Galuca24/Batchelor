import React, { useEffect, useState } from "react";
import axios from "axios";
import InfoCard from "./InfoCard"; 
import {useUser} from "../../../../context/LoginRequired"

const RatingGivenByUser = () => {
    const [missingBooksCount, setMissingBooksCount] = useState(null);
    const user = useUser();

    useEffect(() => {
        const fetchMissingBooks = async () => {
            try {
                const response = await axios.get(`https://localhost:7115/api/v1/rating/getratingsgivenbyuserid/${user.userId}`);
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
            title="Ratings Given To Books"
            value={missingBooksCount}
        />
    );
};

export default RatingGivenByUser;
