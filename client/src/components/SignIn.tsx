import * as React from 'react';
import Avatar from '@mui/material/Avatar';
import Button from '@mui/material/Button';
import CssBaseline from '@mui/material/CssBaseline';
import TextField from '@mui/material/TextField';
import Grid from '@mui/material/Grid';
import Box from '@mui/material/Box';
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';
import Typography from '@mui/material/Typography';
import Container from '@mui/material/Container';
import {useNavigate, Link} from "react-router-dom";
import {useForm, Controller} from 'react-hook-form'
import {useAuth} from "../context/AuthContext";
import {Alert, FormHelperText} from "@mui/material";

export interface LoginFormData {
    email: string;
    password: string;
}

export default function SignIn() {
    const navigate = useNavigate();
    const {handleSubmit, control} = useForm<LoginFormData>({
        defaultValues: {
            email: '',
            password: ''
        }
    });
    const {login, error, clearErrors} = useAuth()

    const submitHandler = (data: LoginFormData) => {
        login(data);
    };

    return (
        <Container component="main" maxWidth="xs">
            <CssBaseline/>
            <Box
                sx={{
                    marginTop: 8,
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center',
                }}
            >
                <Avatar sx={{m: 1, bgcolor: 'secondary.main'}}>
                    <LockOutlinedIcon/>
                </Avatar>
                <Typography component="h1" variant="h5">
                    Sign in
                </Typography>
                <Box component="form" onSubmit={handleSubmit(submitHandler)} noValidate sx={{mt: 1}}>
                    <Controller name="email" control={control} render={({field}) => (
                        <TextField
                            {...field}
                            autoFocus
                            margin="normal"
                            fullWidth
                            id="email"
                            label="Email Address"
                            error={!!error}
                            onChange={e => {
                                clearErrors();
                                field.onChange(e);
                            }}
                        />
                    )}/>
                    <Controller name="password" control={control} render={({field}) => (
                        <TextField
                            {...field}
                            margin="normal"
                            fullWidth
                            label="Password"
                            type="password"
                            id="password"
                            error={!!error}
                            onChange={e => {
                                error && clearErrors();
                                field.onChange(e);
                            }}
                        />
                    )}/>
                    <FormHelperText
                        className="Mui-error"
                    >
                        {error ? 'Invalid credentials' : ''}
                    </FormHelperText>
                    <Button
                        type="submit"
                        fullWidth
                        variant="contained"
                        sx={{mt: 3, mb: 2}}
                    >
                        Sign In
                    </Button>
                    <Grid container>
                        <Grid item xs>
                            <Typography textAlign="center"
                                        component={Link}
                                        sx={{color: "#1976d2"}}
                                        variant="body2"
                                        to='/login'>Forgot password?
                            </Typography>
                        </Grid>
                        <Grid item>
                            <Typography textAlign="center"
                                        component={Link}
                                        sx={{color: "#1976d2"}}
                                        variant="body2"
                                        to='/register'>Don't have an account? Sign Up
                            </Typography>
                        </Grid>
                    </Grid>
                </Box>
            </Box>
        </Container>
    );
}