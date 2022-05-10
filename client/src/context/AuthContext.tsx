import React, {createContext, ReactNode, useContext, useEffect, useMemo, useState} from 'react'
import axios from "redaxios";
import {LoginFormData} from "../components/SignIn";
import {useLocation, useNavigate} from "react-router-dom";

interface User {
    userId: number;
    firstName: string;
    lastName: string;
    email: string;
    phoneNumber: string;
    company: {
        companyId: number;
        companyName: string;
        companyDescription: string;
    }
    role: {
        roleId: number;
        roleType: string;
    }
    country: {
        countryId: number;
        countryName: string;
    }
}

interface AuthContextData {
    user: User;
    authenticated: boolean;
    loading: boolean;
    error: any;
    login: (userData: LoginFormData) => void;
    logout: () => void;
    clearErrors: () => void;
}

const AuthContext = createContext<AuthContextData>({} as AuthContextData)

export const AuthProvider = ({children}: { children: ReactNode }) => {
    const [user, setUser] = useState<User>({} as User);
    const [error, setError] = useState<any>();
    const [loading, setLoading] = useState(true);
    const [authenticated, setAuthenticated] = useState(false);

    const navigate = useNavigate();
    const location = useLocation();

    useEffect(() => {
        axios.get('https://localhost:7166/api/Users/user')
            .then(res => {
                setUser(res.data);
                setAuthenticated(true);
            })
            .catch(error => navigate('/login'));
    }, []);

    useEffect(() => {
        setError(null);
    }, [location.pathname]);

    const login = (userData: LoginFormData) => {
        setLoading(true);

        axios.post('https://localhost:7166/api/Users/login', userData)
            .then(res => {
                setUser(res.data);
                setAuthenticated(true);

                navigate('/');
            })
            .catch(error => setError(error))
            .finally(() => setLoading(false));
    }

    const logout = () => {
        axios.get('https://localhost:7166/api/Users/logout')
            .then(res => {
                setUser({} as User);
                setAuthenticated(false);

                navigate('/login');
            });
    }

    const clearErrors = () => {
        setError(null);
    }

    const memoedValue = useMemo(() => ({
        user,
        authenticated,
        loading,
        error,
        login,
        logout,
        clearErrors
    }), [user, authenticated, loading, error]);

    return (
        <AuthContext.Provider value={memoedValue}>
            {children}
        </AuthContext.Provider>
    )
}

export const useAuth = () => {
    return useContext(AuthContext);
}