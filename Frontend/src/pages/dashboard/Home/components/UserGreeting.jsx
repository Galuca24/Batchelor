import React from 'react';
const UserGreeting = ({ userName }) => {
  const formatDate = (date) => {
    const options = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric', hour: '2-digit', minute: '2-digit' };
    return new Date(date).toLocaleDateString("en-US", options);
  };

  return (
    <div className="mb-6">
      <h1 className="text-5xl font-bold text-black">Hello, <span className="text-green-600">{userName}!</span></h1>
      {}
      <p className="text-xl text-gray-600">{formatDate(new Date())}</p>
      {}
    </div>
  );
};

export default UserGreeting;
