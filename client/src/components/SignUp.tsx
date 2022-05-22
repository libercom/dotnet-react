import Avatar from '@mui/material/Avatar';
import Button from '@mui/material/Button';
import CssBaseline from '@mui/material/CssBaseline';
import TextField from '@mui/material/TextField';
import Grid from '@mui/material/Grid';
import Box from '@mui/material/Box';
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';
import Typography from '@mui/material/Typography';
import Container from '@mui/material/Container';
import axios from "redaxios";
import {useNavigate, Link} from "react-router-dom";
import {FormControl, FormHelperText, InputLabel, MenuItem, Select} from "@mui/material";
import React, {useState, useEffect} from "react";
import {useForm, Controller, SubmitHandler} from "react-hook-form"
import {yupResolver} from "@hookform/resolvers/yup";
import * as yup from "yup"
import useDebounce from "../hooks/useDebounce";

interface CountryData {
    countryId: number;
    countryName: string;
}

interface CompanyData {
    companyId: number;
    companyName: string;
    companyDescription: string;
}

interface RegisterFormData {
    firstName: string;
    lastName: string
    email: string;
    phoneNumber: string;
    companyId: string | number;
    countryId: string | number;
    roleId: number;
    password: string;
}

const schema = yup.object().shape({
    firstName: yup.string().required('Firstname is required'),
    lastName: yup.string().required('Last name is required'),
    email: yup.string().email('Invalid email').required('Email is required'),
    phoneNumber: yup.string()
        .required('Phone number is required')
        .min(12, 'Invalid phone number'),
    companyId: yup.number().required('Company is required'),
    countryId: yup.number().required('Country is required'),
    password: yup.string()
        .required('Password is required')
        .min(8, 'Password should be at least 8 characters long')
        .max(20, 'Password should be at most 20 characters long'),
});

export default function SignUp() {
    const [countries, setCountries] = useState<CountryData[]>([]);
    const [companies, setCompanies] = useState<CompanyData[]>([]);
    const [email, setEmail] = useState('');
    const {handleSubmit, setError, clearErrors, formState: {errors}, control} = useForm<RegisterFormData>({
        resolver: yupResolver(schema),
        mode: 'onChange',
        defaultValues: {
            firstName: '',
            lastName: '',
            email: '',
            phoneNumber: '',
            password: ''
        }
    });
    const navigate = useNavigate();
    const debouncedValue = useDebounce(email, 200);

    useEffect(() => {
        let isMounted = true;

        axios.get(`https://localhost:7166/api/Users/check/${email}`)
            .then(res => {
                if (res.data) {
                    if (isMounted) {
                        setError('email', {type: 'custom', message: 'Email already exists'});
                    }
                } else {
                    if (errors.email?.type === 'custom') {
                        clearErrors('email');
                    }
                }
            })
            .catch(error => console.log(error));

        return () => {
            isMounted = false;
        }
    }, [debouncedValue, clearErrors, setError]);

    useEffect(() => {
        axios.get('https://localhost:7166/api/Countries')
            .then(res => setCountries(res.data))
            .catch(error => console.log(error));
        axios.get('https://localhost:7166/api/Companies')
            .then(res => setCompanies(res.data))
            .catch(error => console.log(error));
    }, []);

    const submitHandler: SubmitHandler<RegisterFormData> = (data: RegisterFormData) => {
        axios.post('https://localhost:7166/api/Users/register', {
            firstName: data.firstName,
            lastName: data.lastName,
            email: data.email,
            phoneNumber: data.phoneNumber,
            companyId: data.companyId,
            countryId: data.countryId,
            roleId: 1,
            password: data.password
        }).then(res => console.log(res))
            .catch(error => console.log(error))

        navigate('/login');
    };

    const emailChangeHandler = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        setEmail(e.target.value);
    }

    return (
        <Container component="main" maxWidth="xs">
            <CssBaseline/>
            <Box
                sx={{
                    marginTop: 3,
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center',
                }}
            >
                <Avatar sx={{m: 1, bgcolor: 'secondary.main'}}>
                    <LockOutlinedIcon/>
                </Avatar>
                <Typography component="h1" variant="h5">
                    Sign up
                </Typography>
                <Box component="form" onSubmit={handleSubmit(submitHandler)} sx={{mt: 3}}>
                    <Grid container spacing={2}>
                        <Grid item xs={12} sm={6}>
                            <Controller name="firstName" control={control} render={({field}) => (
                                <TextField
                                    {...field}
                                    autoFocus
                                    fullWidth
                                    id="firstName"
                                    label="First Name"
                                    error={!!errors.firstName}
                                    helperText={errors.firstName ? errors.firstName.message : ''}
                                />
                            )}
                            />
                        </Grid>
                        <Grid item xs={12} sm={6}>
                            <Controller name="lastName" control={control} render={({field}) => (
                                <TextField
                                    {...field}
                                    fullWidth
                                    id="lastName"
                                    label="Last Name"
                                    name="lastName"
                                    error={!!errors.lastName}
                                    helperText={errors.lastName ? errors.lastName.message : ''}
                                />
                            )}
                            />
                        </Grid>
                        <Grid item xs={12}>
                            <Controller name="email" control={control} render={({field}) => (
                                <TextField
                                    {...field}
                                    fullWidth
                                    id="email"
                                    label="Email Address"
                                    error={!!errors.email}
                                    helperText={errors.email ? errors.email.message : ''}
                                    onChange={e => {
                                        emailChangeHandler(e);
                                        field.onChange(e);
                                    }}
                                />
                            )}
                            />
                        </Grid>
                        <Grid item xs={12}>
                            <Controller name="phoneNumber" control={control} render={({field}) => (
                                <TextField
                                    {...field}
                                    fullWidth
                                    id="phoneNumber"
                                    label="Phone Number"
                                    error={!!errors.phoneNumber}
                                    helperText={errors.phoneNumber ? errors.phoneNumber.message : ''}
                                />
                            )}
                            />
                        </Grid>
                        <Grid item xs={12}>
                            <FormControl fullWidth>
                                <InputLabel className={errors.companyId ? "Mui-error" : ""}
                                            id="companyLabel"
                                >
                                    Company
                                </InputLabel>
                                <Controller name="companyId" control={control} render={({field}) => (
                                    <Select
                                        {...field}
                                        labelId="companyLabel"
                                        id="company"
                                        label="Company"
                                        defaultValue=""
                                        error={!!errors.companyId}
                                    >
                                        {companies && companies.map((company) => (
                                            <MenuItem
                                                key={company.companyId}
                                                value={company.companyId}
                                            >
                                                {company.companyName}
                                            </MenuItem>
                                        ))}
                                    </Select>
                                )}
                                />
                                <FormHelperText
                                    className="Mui-error"
                                >
                                    {errors.companyId ? errors.companyId.message : ''}
                                </FormHelperText>
                            </FormControl>
                        </Grid>
                        <Grid item xs={12}>
                            <FormControl fullWidth>
                                <InputLabel className={errors.countryId ? "Mui-error" : ""}
                                            id="countryLabel"
                                >
                                    Country
                                </InputLabel>
                                <Controller name="countryId" control={control} render={({field}) => (
                                    <Select
                                        {...field}
                                        labelId="countryLabel"
                                        id="country"
                                        label="Country"
                                        defaultValue=""
                                        error={!!errors.countryId}
                                    >
                                        {countries && countries.map((country) => (
                                            <MenuItem
                                                key={country.countryId}
                                                value={country.countryId}
                                            >
                                                {country.countryName}
                                            </MenuItem>
                                        ))}
                                    </Select>
                                )}
                                />
                                <FormHelperText
                                    className="Mui-error"
                                >
                                    {errors.countryId ? errors.countryId.message : ''}
                                </FormHelperText>
                            </FormControl>
                        </Grid>
                        <Grid item xs={12}>
                            <Controller name="password" control={control} render={({field}) => (
                                <TextField
                                    {...field}
                                    fullWidth
                                    label="Password"
                                    type="password"
                                    id="password"
                                    error={!!errors.password}
                                    helperText={errors.password ? errors.password.message : ''}
                                />
                            )}
                            />
                        </Grid>
                    </Grid>
                    <Button
                        type="submit"
                        fullWidth
                        variant="contained"
                        sx={{mt: 3, mb: 2}}
                    >
                        Sign Up
                    </Button>
                    <Grid container justifyContent="flex-end">
                        <Grid item>
                            <Typography textAlign="center"
                                        component={Link}
                                        sx={{color: "#1976d2"}}
                                        variant="body2"
                                        to='/login'>Already have an account? Sign in
                            </Typography>
                        </Grid>
                    </Grid>
                </Box>
            </Box>
        </Container>
    );
}