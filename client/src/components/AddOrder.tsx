import React, {useEffect, useState} from 'react'
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import DialogContentText from "@mui/material/DialogContentText";
import {FormControl, FormHelperText, InputLabel, Select} from "@mui/material";
import MenuItem from "@mui/material/MenuItem";
import DialogActions from "@mui/material/DialogActions";
import Button from "@mui/material/Button";
import Dialog from "@mui/material/Dialog";
import {useForm, Controller, SubmitHandler} from "react-hook-form"
import {yupResolver} from "@hookform/resolvers/yup";
import * as yup from "yup"
import axios from "redaxios";
import Grid from "@mui/material/Grid";
import Box from "@mui/material/Box";
import TextField from '@mui/material/TextField';
import {AdapterDateFns} from '@mui/x-date-pickers/AdapterDateFns';
import {LocalizationProvider} from '@mui/x-date-pickers/LocalizationProvider';
import {DatePicker} from '@mui/x-date-pickers/DatePicker';

interface CountryData {
    countryId: number;
    countryName: string;
}

interface AddOrderParams {
    openAddOrder: boolean;
    handleAddOrderClose: () => void;
}

interface AddOrderFormData {
    shipmentDate: string;
    arrivalDate: string;
    payment: number;
    cargoTypeId: string | number;
    paymentMethodId: string | number;
    sendingCountryId: string | number;
    destinationCountryId: string | number;
}

const schema = yup.object().shape({
    shipmentDate: yup.string().required('Shipment Date is required'),
    arrivalDate: yup.string().required('Arrival Date is required'),
    payment: yup.number().required('Payment is required'),
    cargoTypeId: yup.number().required('Cargo Type is required'),
    paymentMethodId: yup.number().required('Payment Method is required'),
    sendingCountryId: yup.number().required('Sending Country is required'),
    destinationCountryId: yup.number().required('Destination Country is required')
});

export default function AddOrder(params: AddOrderParams) {
    const {openAddOrder, handleAddOrderClose} = params;
    const [countries, setCountries] = useState<CountryData[]>([]);
    const {handleSubmit, setError, clearErrors, formState: {errors}, control} = useForm<AddOrderFormData>({
        resolver: yupResolver(schema),
        mode: 'onChange',
        defaultValues: {
            shipmentDate: '',
            arrivalDate: '',
            payment: 0,
        }
    });

    useEffect(() => {
        if (openAddOrder) {
            axios.get('https://localhost:7166/api/Countries')
                .then(res => setCountries(res.data))
                .catch(error => console.log(error));
        }
    }, [openAddOrder]);

    const submitHandler: SubmitHandler<AddOrderFormData> = (data: AddOrderFormData) => {
        axios.post('https://localhost:7166/api/Orders', {
            // todo
        }).then(res => console.log(res))
            .catch(error => console.log(error));
    }

    return (
        <Dialog open={openAddOrder} onClose={handleAddOrderClose}>
            <DialogTitle>Add Order</DialogTitle>
            <DialogContent>
                <DialogContentText>
                    Fill the following form to create a new order
                </DialogContentText>
                <Box component="form" onSubmit={handleSubmit(submitHandler)} sx={{mt: 3}}>
                    <Grid container spacing={2}>
                        <Grid item xs={12}>
                            <FormControl fullWidth>
                                <InputLabel className={errors.sendingCountryId ? "Mui-error" : ""}
                                            id="sendingCountryLabel"
                                >
                                    Sending Country
                                </InputLabel>
                                <Controller name="sendingCountryId" control={control} render={({field}) => (
                                    <Select
                                        {...field}
                                        labelId="sendingCountryLabel"
                                        id="sendingCountry"
                                        label="Sending Country"
                                        defaultValue=""
                                        error={!!errors.sendingCountryId}
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
                                    {errors.sendingCountryId ? errors.sendingCountryId.message : ''}
                                </FormHelperText>
                            </FormControl>
                        </Grid>
                        <Grid item xs={12}>
                            <LocalizationProvider dateAdapter={AdapterDateFns}>
                                <DatePicker
                                    label="Basic example"
                                    value={value}
                                    onChange={(newValue) => {
                                        setValue(newValue);
                                    }}
                                    renderInput={(params) => <TextField {...params} />}
                                />
                            </LocalizationProvider>
                        </Grid>
                        <Button
                            type="submit"
                            fullWidth
                            variant="contained"
                            sx={{mt: 3, mb: 2, ml: 2}}
                        >
                            Add Order
                        </Button>
                    </Grid>
                </Box>
            </DialogContent>
            <DialogActions>
                <Button onClick={handleAddOrderClose}>Close</Button>
            </DialogActions>
        </Dialog>
    )
}