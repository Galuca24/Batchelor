import React, { useEffect, useState } from "react";
import axios from "axios";
import { Link } from "react-router-dom";
import { Avatar } from "@material-tailwind/react";

const s3BaseUrl = "https://galabucket.s3.eu-north-1.amazonaws.com/";

const RecentCheckouts = () => {
    const [checkouts, setCheckouts] = useState([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [itemsPerPage] = useState(5);

    useEffect(() => {
        const fetchRecentCheckouts = async () => {
            try {
                const response = await axios.get('https://localhost:7115/api/v1/Books/recent-checkouts');
                const checkoutsWithPhotos = await Promise.all(response.data.map(async (checkout) => {
                    try {
                        const photoResponse = await axios.get(`https://localhost:7115/api/Cloud/get-user-photo?userId=${checkout.userId}`);
                        const fullPhotoUrl = s3BaseUrl + photoResponse.data.userPhotoUrl;
                        return { ...checkout, userPhotoUrl: fullPhotoUrl };
                    } catch (photoError) {
                        console.error('Error fetching photo: ', photoError);
                        return { ...checkout, userPhotoUrl: "/img/placeholder.png" };
                    }
                }));
                setCheckouts(checkoutsWithPhotos);
            } catch (error) {
                console.error('Error fetching recent checkouts: ', error);
            }
        };

        fetchRecentCheckouts();
    }, []);

    const pageCount = Math.ceil(checkouts.length / itemsPerPage);

    const changePage = (newPage) => {
        setCurrentPage(newPage);
    };

    const currentCheckouts = checkouts.slice(
        (currentPage - 1) * itemsPerPage,
        currentPage * itemsPerPage
    );

    return (
        <div className="overflow-x-auto relative shadow-md sm:rounded-lg bg-surface-white">
            <div className="flex justify-between items-center">
                <h2 className="text-base ml-5 mt-5 text-surface-black font-semibold leading-tight">Recent Check-outs</h2>
                <Link to="/full-checkouts" className="text-surface-dark-green hover:text-surface-light-green mr-6">View All</Link>
            </div>
            <table className="w-full text-sm text-left text-gray-500 mt-5">
                <thead className="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
                    <tr>
                        <th scope="col" className="py-3 px-6">User</th>
                        <th scope="col" className="py-3 px-6">Book Title</th>
                        <th scope="col" className="py-3 px-6">Author</th>
                        <th scope="col" className="py-3 px-6">Issue Date</th>
                        <th scope="col" className="py-3 px-6">Return Date</th>
                    </tr>
                </thead>
                <tbody>
                    {currentCheckouts.map((checkout, index) => (
                        <tr key={index} className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
                            <td className="py-4 px-6 flex items-center">
                                <Avatar src={checkout.userPhotoUrl || "/img/placeholder.png"} size="sm" />
                                <Link to={`/dashboard/profile/${checkout.userId}`} className="ml-3 text-blue-600 hover:text-blue-800">{checkout.userName}</Link>
                            </td>
                            <td className="py-4 px-6">{checkout.bookTitle}</td>
                            <td className="py-4 px-6">{checkout.author}</td>
                            <td className="py-4 px-6">{new Date(checkout.issueDate).toLocaleDateString()}</td>
                            <td className="py-4 px-6">{new Date(checkout.returnDate).toLocaleDateString()}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
            
        </div>
    );
};

export default RecentCheckouts;
