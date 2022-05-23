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
import {useAuth} from "../context/AuthContext";

interface CountryData {
    countryId: number;
    countryName: string;
}

interface PaymentMethodData {
    paymentMethodId: number;
    paymentMethodName: string;
}

interface CargoTypeData {
    cargoTypeId: number;
    cargoTypeName: string;
}

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

interface OrderData {
    id: number;
    user: User;
    shipmentDate: Date;
    arrivalDate: Date;
    cargoType: string;
    payment: number;
    paymentMethod: string;
    sendingCountry: string;
    destinationCountry: string;
    cargoTypeId: number;
    paymentMethodId: number;
    sendingCountryId: number;
    destinationCountryId: number;
}

interface UpdateOrderParams {
    order: OrderData;
    openUpdateOrder: boolean;
    handleUpdateOrderClose: () => void;
    handleRefreshPage: () => void;
}

interface AddOrderFormData {
    shipmentDate: Date | null;
    arrivalDate: Date | null;
    payment: string | number;
    cargoTypeId: string | number;
    paymentMethodId: string | number;
    sendingCountryId: string | number;
    destinationCountryId: string | number;
}

const schema = yup.object().shape({
    shipmentDate: yup.date().nullable().required('Shipment Date is required'),
    arrivalDate: yup.date().nullable().required('Arrival Date is required'),
    payment: yup.number().required('Payment is required').min(50, 'Payment should be at least 50$').typeError('Payment should be a number'),
    cargoTypeId: yup.number().required('Cargo Type is required'),
    paymentMethodId: yup.number().required('Payment Method is required'),
    sendingCountryId: yup.number().required('Sending Country is required'),
    destinationCountryId: yup.number().required('Destination Country is required')
});

export default function UpdateOrder(params: UpdateOrderParams) {
    const {openUpdateOrder, order, handleUpdateOrderClose, handleRefreshPage} = params;
    const {user} = useAuth();
    const {
        handleSubmit,
        formState: {errors},
        control,
        reset
    } = useForm<AddOrderFormData>({
        resolver: yupResolver(schema),
        mode: 'onChange',
        defaultValues: {
            shipmentDate: null,
            arrivalDate: null,
            payment: '',
            paymentMethodId: '',
            cargoTypeId: '',
            destinationCountryId: '',
            sendingCountryId: '',
        }
    });

    const [loading, setLoading] = useState(false);
    const [countries, setCountries] = useState<CountryData[]>([]);
    const [paymentMethods, setPaymentMethods] = useState<PaymentMethodData[]>([]);
    const [cargoTypes, setCargoTypes] = useState<CargoTypeData[]>([]);

    useEffect(() => {
        if (openUpdateOrder) {
            setLoading(true);

            Promise.all([
                axios.get('https://localhost:7166/api/Countries'),
                axios.get('https://localhost:7166/api/PaymentMethods'),
                axios.get('https://localhost:7166/api/CargoTypes')
            ])
                .then(values => {
                    setCountries(values[0].data);
                    setPaymentMethods(values[1].data);
                    setCargoTypes(values[2].data);
                    reset({
                        shipmentDate: order.shipmentDate,
                        arrivalDate: order.arrivalDate,
                        payment: order.payment,
                        paymentMethodId: order.paymentMethodId,
                        cargoTypeId: order.cargoTypeId,
                        destinationCountryId: order.destinationCountryId,
                        sendingCountryId: order.sendingCountryId,
                    });
                })
                .catch(error => console.log(error))
                .finally(() => setLoading(false));

        }
    }, [openUpdateOrder]);

    const submitHandler: SubmitHandler<AddOrderFormData> = (data: AddOrderFormData) => {
        axios.put(`https://localhost:7166/api/Orders/${order.id}`, {
            userId: user.role.roleId,
            shipmentDate: data.shipmentDate?.toISOString(),
            arrivalDate: data.arrivalDate?.toISOString(),
            payment: data.payment,
            cargoTypeId: data.cargoTypeId,
            paymentMethodId: data.paymentMethodId,
            sendingCountryId: data.sendingCountryId,
            destinationCountryId: data.destinationCountryId,
        }).then(() => {
            handleDialogClose();
            handleRefreshPage();
        })
            .catch(error => console.log(error));
    }

    const handleDialogClose = () => {
        handleUpdateOrderClose();
    }

    return (
        <Dialog open={openUpdateOrder} onClose={handleDialogClose}>
            <DialogTitle>Update Order</DialogTitle>
            <DialogContent>
                <DialogContentText>
                    Change the following form to update an existing order
                </DialogContentText>
                {!loading &&
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
                                <FormControl fullWidth>
                                    <InputLabel className={errors.sendingCountryId ? "Mui-error" : ""}
                                                id="destinationCountryLabel"
                                    >
                                        Destination Country
                                    </InputLabel>
                                    <Controller name="destinationCountryId" control={control} render={({field}) => (
                                        <Select
                                            {...field}
                                            labelId="destinationCountryLabel"
                                            id="destinationCountryId"
                                            label="Destination Country"
                                            error={!!errors.destinationCountryId}
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
                                        {errors.destinationCountryId ? errors.destinationCountryId.message : ''}
                                    </FormHelperText>
                                </FormControl>
                            </Grid>
                            <Grid item xs={12}>
                                <Controller name="payment" control={control} render={({field}) => (
                                    <TextField
                                        {...field}
                                        fullWidth
                                        label="Payment"
                                        id="payment"
                                        error={!!errors.payment}
                                        helperText={errors.payment ? errors.payment.message : ''}
                                    />
                                )}
                                />
                            </Grid>
                            <Grid item xs={12}>
                                <FormControl fullWidth>
                                    <InputLabel className={errors.sendingCountryId ? "Mui-error" : ""}
                                                id="cargoTypeLabel"
                                    >
                                        Cargo Type
                                    </InputLabel>
                                    <Controller name="cargoTypeId" control={control} render={({field}) => (
                                        <Select
                                            {...field}
                                            labelId="cargoTypeLabel"
                                            id="cargoType"
                                            label="Cargo Type"
                                            error={!!errors.cargoTypeId}
                                        >
                                            {cargoTypes && cargoTypes.map((cargoType) => (
                                                <MenuItem
                                                    key={cargoType.cargoTypeId}
                                                    value={cargoType.cargoTypeId}
                                                >
                                                    {cargoType.cargoTypeName}
                                                </MenuItem>
                                            ))}
                                        </Select>
                                    )}
                                    />
                                    <FormHelperText
                                        className="Mui-error"
                                    >
                                        {errors.cargoTypeId ? errors.cargoTypeId.message : ''}
                                    </FormHelperText>
                                </FormControl>
                            </Grid>
                            <Grid item xs={12}>
                                <FormControl fullWidth>
                                    <InputLabel className={errors.paymentMethodId ? "Mui-error" : ""}
                                                id="paymentMethodLabel"
                                    >
                                        Payment Method
                                    </InputLabel>
                                    <Controller name="paymentMethodId" control={control} render={({field}) => (
                                        <Select
                                            {...field}
                                            labelId="paymentMethodLabel"
                                            id="paymentMethod"
                                            label="Payment Method"
                                            error={!!errors.paymentMethodId}
                                        >
                                            {paymentMethods && paymentMethods.map((paymentMethod) => (
                                                <MenuItem
                                                    key={paymentMethod.paymentMethodId}
                                                    value={paymentMethod.paymentMethodId}
                                                >
                                                    {paymentMethod.paymentMethodName}
                                                </MenuItem>
                                            ))}
                                        </Select>
                                    )}
                                    />
                                    <FormHelperText
                                        className="Mui-error"
                                    >
                                        {errors.paymentMethodId ? errors.paymentMethodId.message : ''}
                                    </FormHelperText>
                                </FormControl>
                            </Grid>
                            <Grid item xs={6}>
                                <LocalizationProvider dateAdapter={AdapterDateFns}>
                                    <Controller name="shipmentDate" control={control} render={({field}) => (
                                        <DatePicker
                                            {...field}
                                            label="Shipment Date"
                                            renderInput={(params) => (
                                                <TextField fullWidth {...params} error={!!errors.shipmentDate}
                                                           helperText={errors.shipmentDate ? errors.shipmentDate.message : ''}/>
                                            )}
                                        />
                                    )}/>
                                </LocalizationProvider>
                            </Grid>
                            <Grid item xs={6}>
                                <LocalizationProvider dateAdapter={AdapterDateFns}>
                                    <Controller name="arrivalDate" control={control} render={({field}) => (
                                        <DatePicker
                                            {...field}
                                            label="Arrival Date"
                                            renderInput={(params) => (
                                                <TextField fullWidth {...params} error={!!errors.arrivalDate}
                                                           helperText={errors.arrivalDate ? errors.arrivalDate.message : ''}/>
                                            )}
                                        />
                                    )}/>
                                </LocalizationProvider>
                            </Grid>
                        </Grid>
                        <Button
                            fullWidth
                            type="submit"
                            variant="contained"
                            sx={{mt: 3, mb: 2}}
                        >
                            Update Order
                        </Button>
                    </Box>
                }
            </DialogContent>
            <DialogActions>
                <Button onClick={handleUpdateOrderClose}>Close</Button>
            </DialogActions>
        </Dialog>
    )
}