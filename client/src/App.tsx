import React, {useEffect} from 'react';
import './App.css';
import SignIn from './components/SignIn'
import Navbar from './components/Navbar';
import {Route, Routes, BrowserRouter as Router, Navigate} from 'react-router-dom'
import Home from "./components/Home";
import SignUp from "./components/SignUp";
import {AuthProvider, useAuth} from "./context/AuthContext";
import {Routing} from "./Routing";

function App() {
    const {authenticated} = useAuth();

    return (
        <Router>
            <AuthProvider>
                <Navbar/>
                <Routing/>
            </AuthProvider>
        </Router>
    );
}

export default App;
