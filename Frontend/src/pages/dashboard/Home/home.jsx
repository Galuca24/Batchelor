// Home.js
import React from "react";
import { useUser } from "../../../context/LoginRequired";
import HomeForAdmin from "./components/HomeForAdmin";
import HomeForUser from "./components/HomeForUser";

export function Home() {
  const user = useUser();

  if (user.role === 'Admin') {
    return <HomeForAdmin />;
  } else {
    return <HomeForUser />;
  }
}

export default Home;
