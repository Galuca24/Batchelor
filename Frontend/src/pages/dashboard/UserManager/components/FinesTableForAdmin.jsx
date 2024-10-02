import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { toast } from 'react-toastify';
import { Avatar } from "@material-tailwind/react";
import { EyeIcon, WalletIcon } from '@heroicons/react/24/solid';
import FineDetailsModal from "./FineDetailsModal"

const s3BaseUrl = "https://galabucket.s3.eu-north-1.amazonaws.com/";

const FinesTableForAdmin = () => {
    const [fines, setFines] = useState([]);
    const [selectedFine, setSelectedFine] = useState(null);
    const [isModalOpen, setIsModalOpen] = useState(false);

    const handleViewFine = (fine) => {
        setSelectedFine(fine);
        setIsModalOpen(true);
    };

    const closeModal = () => {
        setIsModalOpen(false);
    };

    function truncateString(str, num) {
        if (str.length > num) {
          return str.slice(0, num) + "...";
        } else {
          return str;
        }
      }
    useEffect(() => {
        const fetchFines = async () => {
            try {
                const finesResponse = await axios.get('https://localhost:7115/api/v1/fines/getallfines');
                const finesWithData = await Promise.all(finesResponse.data.map(async (fine) => {

                    // Fetch user and book details in parallel
                    const [userResponse, bookResponse] = await Promise.all([
                        axios.get(`https://localhost:7115/api/v1/users/${fine.userId}`),
                        axios.get(`https://localhost:7115/api/v1/books/getbookbyid/${fine.bookId}`)
                    ]);
                    console.log(userResponse);

                    // Attempt to fetch the user's photo
                    let fullPhotoUrl = "/img/placeholder.png"; // Default photo if fetch fails
                    try {
                        const photoResponse = await axios.get(`https://localhost:7115/api/Cloud/get-user-photo?userId=${fine.userId}`);
                        fullPhotoUrl = s3BaseUrl + photoResponse.data.userPhotoUrl;
                    } catch (photoError) {
                        console.error('Error fetching photo:', photoError);
                        // Use default image if there's an error
                    }

                    return { ...fine, user: { ...userResponse.data.user, photoUrl: fullPhotoUrl }, book: bookResponse.data };
                }));
                setFines(finesWithData);
            } catch (error) {
                console.error('Failed to fetch fines:', error);
                toast.error("Failed to fetch fines");
            }
        };

        fetchFines();
    }, []);

    return (
        <div className="overflow-x-auto relative shadow-md sm:rounded-lg bg-surface-white mt-7">
            <table className="w-full text-sm text-left text-gray-500">
                <thead className="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
                    <tr>
                        <th scope="col" className="py-3 px-6">User</th>
                        <th scope="col" className="py-3 px-6">Book</th>
                        <th scope="col" className="py-3 px-6">Overdue Days</th>
                        <th scope="col" className="py-3 px-6">Amount</th>
                        <th scope="col" className="py-3 px-6">Status</th>
                        <th scope="col" className="py-3 px-6">Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {fines.map((fine, index) => (
                        <tr key={index} className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
                            <td className="py-4 px-6">
                                <div className="flex items-center">
                                    <Avatar src={fine.user.photoUrl} size="sm" />
                                    <span className="ml-3">{fine.user.name}</span>
                                </div>
                            </td>
                            <td className="py-4 px-6">
                                <div className="flex items-center">
                                    <img src={fine.book.imageUrl} alt={fine.book.title} style={{ width: '50px', height: '50px', objectFit: 'cover' }} />
                                    <span className="ml-3">{truncateString(fine.book.title,25)}</span>
                                </div>
                            </td>
                            <td className="py-4 px-6">{fine.daysOverdue}</td>
                            <td className="py-4 px-6">${fine.amount}</td>
                            <td className="py-4 px-6">
                                <span className={`font-bold ${fine.isPaid ? 'text-green-500' : 'text-red-500'}`}>
                                    {fine.isPaid ? 'Paid' : 'Unpaid'}
                                </span>
                            </td>
                            <td className="py-4 px-6">
                                <div className="group relative">
                                    <EyeIcon onClick={() => handleViewFine(fine)} className="h-5 w-5 text-blue-500 cursor-pointer" />
                                    <span className="absolute w-auto p-2 m-2 min-w-max left-1/2 transform -translate-x-1/2 bottom-full bg-black text-white text-xs rounded-md opacity-0 group-hover:opacity-100 transition-opacity duration-300 ease-in-out">
                                        View Fine Details
                                    </span>
                                </div>
                            </td>
                        </tr>
                    ))}

                </tbody>
            </table>
            <FineDetailsModal open={isModalOpen} onClose={closeModal} fine={selectedFine} />

        </div>
    );
};

export default FinesTableForAdmin
