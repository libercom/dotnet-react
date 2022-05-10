import {Navigate, Route, Routes} from "react-router-dom";
import SignIn from "./components/SignIn";
import SignUp from "./components/SignUp";
import Home from "./components/Home";
import React from "react";
import {useAuth} from "./context/AuthContext";

export const Routing = () => {
    const {authenticated} = useAuth();

    return (
        <Routes>
            <Route path="/login" element={!authenticated ? <SignIn/> : <Navigate replace to="/"/>}/>
            <Route path="/register" element={!authenticated ? <SignUp/> : <Navigate replace to="/"/>}/>
            <Route path="/" element={authenticated ? <Home/> : <Navigate replace to="/login"/>}/>
        </Routes>
    )
}