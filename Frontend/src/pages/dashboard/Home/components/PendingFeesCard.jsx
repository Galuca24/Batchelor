import React, { useEffect, useState, useContext } from "react";
import axios from "axios";
import InfoCard from "./InfoCard"; // Presupunem că ai o componentă InfoCard
import { useUser } from "../../../../context/LoginRequired"; // Sau orice hook/context folosești pentru a obține informații despre user

const PendingFeesCard = () => {
    const [totalFines, setTotalFines] = useState(null);
    const user = useUser();

    useEffect(() => {
        const fetchFines = async () => {
            let url;
            if (user.role === 'Admin') {
                url = 'https://localhost:7115/api/v1/Fines/GetTotalFinesAmount';
            } else {
                url = `https://localhost:7115/api/v1/Fines/getunpaidfinesamountbyuser/${user.userId}`;
            }

            try {
                const response = await axios.get(url, {
                    headers: { Authorization: `Bearer ${user.token}` }
                });

                if (response.status === 200) {
                    setTotalFines(response.data);
                } else {
                    console.error('Failed to fetch fines: ', response.status);
                }
            } catch (error) {
                console.error('Error fetching fines: ', error);
            }
        };

        fetchFines();
    }, [user]);

    if (totalFines === null) {
        return <div>Loading...</div>; // Sau orice altă formă de loading indicator
    }

    return (
        <InfoCard
            title="Pending Fees"
            value={`${totalFines} $`} // Presupunem că amenzile sunt în Bitcoin; ajustează conform valutei tale
            // Adaugă orice alte props necesare pentru stilizare sau comportament
        />
    );
};

export default PendingFeesCard;
