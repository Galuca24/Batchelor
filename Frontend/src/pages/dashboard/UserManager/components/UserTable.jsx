import React, { useState, useEffect } from 'react';
import { Avatar } from "@material-tailwind/react";
import { TrashIcon, EyeIcon, MinusCircleIcon } from "@heroicons/react/24/solid";
import { useNavigate } from 'react-router-dom';
import DeleteUserConfirmationDialog from './DeleteUserConfirmationDialog';
import axios from 'axios';
import { toast } from 'react-toastify';

const UserTable = ({ users, onDelete }) => {
  const navigate = useNavigate();
  const [isDialogOpen, setDialogOpen] = useState(false);
  const [selectedUserId, setSelectedUserId] = useState(null);
  const [selectedUserName, setSelectedUserName] = useState("");
  const [usersWithFines, setUsersWithFines] = useState(users);

  const handleViewProfile = (userId) => {
    navigate(`/dashboard/profile/${userId}`);
  };


  useEffect(() => {
    const fetchUnpaidFines = async () => {
      const updatedUsers = await Promise.all(users.map(async user => {
        try {
          const response = await axios.get(`https://localhost:7115/api/v1/fines/getunpaidfinesamountbyuser/${user.userId}`);
          return { ...user, unpaidFines: response.data };
        } catch (error) {
          console.error('Failed to fetch fines for user:', user.userId, error);
          return { ...user, unpaidFines: "Error" };
        }
      }));
      setUsersWithFines(updatedUsers);
    };
    fetchUnpaidFines();
  }, [users]);

  const handleDelete = (userId, userName) => {
    setSelectedUserId(userId);
    setSelectedUserName(userName);
    setDialogOpen(true);
  };
  
  const confirmDelete = async () => {
    setDialogOpen(false);
    try {
      const response = await axios.delete(`https://localhost:7115/api/v1/Users/${selectedUserId}`);
      if (response.status === 200 || response.status === 201) {
        toast.success("User deleted successfully");
        onDelete(selectedUserId);
      } else {
        toast.error("Delete failed");
      }
    } catch (error) {
      console.error('Failed to delete the user:', error);
      toast.error("Delete failed with error");
    }
  };
  
  return (
    <div className="overflow-x-auto relative shadow-md sm:rounded-lg bg-surface-white">
      <DeleteUserConfirmationDialog
        open={isDialogOpen}
        onClose={() => setDialogOpen(false)}
        onConfirm={confirmDelete}
        userName={selectedUserName}
      />
      <table className="w-full text-sm text-left text-gray-500">
        <thead className="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
          <tr>
            <th scope="col" className="py-3 px-6">User</th>
            <th scope="col" className="py-3 px-6">Email</th>
            <th scope="col" className="py-3 px-6">Fines Amount to pay</th>
            <th scope="col" className="py-3 px-6">Actions</th>
          </tr>
        </thead>
        <tbody>
        {usersWithFines.map(user => (
            <tr key={user.userId}>
              <td className="py-4 px-6 flex items-center">
                <Avatar src={user.photoUrl} size="sm" />
                <span className="ml-3">{user.name}</span>
              </td>
              <td className="py-4 px-6">{user.email}</td>
              <td className="py-4 px-6">${user.unpaidFines}</td>
              <td className="py-4 px-6">
                <div className="flex items-center space-x-4">
                  <div className="group relative">
                    <EyeIcon onClick={() => handleViewProfile(user.userId)} className="h-5 w-5 text-blue-500 cursor-pointer" />
                    <span className="absolute w-auto p-2 m-2 min-w-max left-1/2 transform -translate-x-1/2 bottom-full bg-black text-white text-xs rounded-md opacity-0 group-hover:opacity-100 transition-opacity duration-300 ease-in-out">
                      View Profile
                    </span>
                  </div>
                  <div className="group relative">
                    <MinusCircleIcon onClick={() => handleDelete(user.userId, user.name)} className="h-5 w-5 text-red-500 cursor-pointer" />
                    <span className="absolute w-auto p-2 m-2 min-w-max left-1/2 transform -translate-x-1/2 bottom-full bg-black text-white text-xs rounded-md opacity-0 group-hover:opacity-100 transition-opacity duration-300 ease-in-out">
                      Delete
                    </span>
                  </div>
                </div>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default UserTable;
