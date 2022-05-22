import React, {FormEvent, useEffect, useState} from 'react'
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Paper from '@mui/material/Paper';
import {FormControl, InputLabel, Select, SelectChangeEvent, Slider, TablePagination} from "@mui/material";
import axios from 'redaxios'
import Container from "@mui/material/Container";
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogContentText from '@mui/material/DialogContentText';
import DialogTitle from '@mui/material/DialogTitle';
import Button from "@mui/material/Button";
import MenuItem from "@mui/material/MenuItem";
import Box from "@mui/material/Box";
import {useAuth} from "../context/AuthContext";
import AddOrder from "./AddOrder";

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

interface OrderRequestData {
    orderId: number;
    userId: string;
    user: User;
    shipmentDate: Date;
    arrivalDate: Date;
    cargoTypeId: number;
    cargoType: {
        cargoTypeId: number;
        cargoTypeName: string;
    }
    payment: number;
    paymentMethodId: number;
    paymentMethod: {
        paymentMethodId: number;
        paymentMethodName: string;
    }
    sendingCountryId: string;
    sendingCountry: {
        countryId: number;
        countryName: string
    }
    destinationCountryId: number;
    destinationCountry: {
        countryId: number;
        countryName: string;
    }
}

interface OrderData {
    id: number;
    userName: string;
    shipmentDate: Date;
    arrivalDate: Date;
    cargoType: string;
    payment: number;
    paymentMethod: string;
    sendingCountry: string;
    destinationCountry: string;
}

interface CountryData {
    countryId: number;
    countryName: string;
}

function createOrderData(order: OrderRequestData): OrderData {
    return {
        id: order.orderId,
        userName: order.user.firstName + ' ' + order.user.lastName,
        shipmentDate: new Date(order.shipmentDate),
        arrivalDate: new Date(order.arrivalDate),
        cargoType: order.cargoType.cargoTypeName,
        payment: order.payment,
        paymentMethod: order.paymentMethod.paymentMethodName,
        sendingCountry: order.sendingCountry.countryName,
        destinationCountry: order.destinationCountry.countryName,
    };
}

export default function Home() {
    const {user} = useAuth();
    const [rows, setRows] = useState<OrderData[]>([] as OrderData[]);
    const [page, setPage] = useState(0);
    const [count, setCount] = useState(0)
    const [rowsPerPage, setRowsPerPage] = useState(5);

    const [openSort, setOpenSort] = useState(false);
    const [sortCriteria, setSortCriteria] = useState('none');
    const [sortType, setSortType] = useState('asc');

    const [openFilter, setOpenFilter] = useState(false);
    const [filterSendingCountry, setFilterSendingCountry] = useState(0);
    const [filterDestinationCountry, setFilterDestinationCountry] = useState(0);

    const [openAddOrder, setOpenAddOrder] = useState(false);

    const [countries, setCountries] = useState<CountryData[]>([] as CountryData[]);

    useEffect(() => {
        axios.get(`https://localhost:7166/api/Orders?pageNumber=${page + 1}&pageSize=${rowsPerPage}&sortCriteria=${sortCriteria}&sortType=${sortType}&destinationCountry=${filterDestinationCountry}&sendingCountry=${filterSendingCountry}`)
            .then(res => {
                let data = res.data.orders as OrderRequestData[];
                let orderData = data.map(order => createOrderData(order));

                setCount(res.data.count);
                setRows(orderData);
            })
            .catch(error => console.log(error));
    }, [page, rowsPerPage, sortCriteria, sortType, filterSendingCountry, filterDestinationCountry]);

    useEffect(() => {
        if (openFilter) {
            axios.get('https://localhost:7166/api/Countries')
                .then(res => setCountries(res.data))
                .catch(error => console.log(error));
        }
    }, [openFilter])

    const handlePageChange = (event: unknown, newPage: number) => {
        setPage(newPage);
    }

    const handleRowsPerPageChange = (e: React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>) => {
        setRowsPerPage(parseInt(e.target.value, 10));
        setPage(0);
    }

    const handleSortClose = () => {
        setOpenSort(false);
    }

    const handleSortOpen = () => {
        setOpenSort(true);
    }

    const handleSortCriteriaChange = (e: SelectChangeEvent<unknown>) => {
        setSortCriteria(e.target.value as string);
    }

    const handleSortTypeChange = (e: SelectChangeEvent<unknown>) => {
        setSortType(e.target.value as string);
    }

    const handleSendingCountryChange = (e: SelectChangeEvent<unknown>) => {
        setFilterSendingCountry(e.target.value as number);
    }

    const handleDestinationCountryChange = (e: SelectChangeEvent<unknown>) => {
        setFilterDestinationCountry(e.target.value as number);
    }

    const handleFilterClose = () => {
        setOpenFilter(false);
    }

    const handleFilterOpen = () => {
        setOpenFilter(true);
    }

    const handleFilterClear = () => {
        setFilterSendingCountry(0);
        setFilterDestinationCountry(0);
    }

    const handleAddOrderClose = () => {
        setOpenAddOrder(false);
    }

    const handleAddOrderOpen = () => {
        setOpenAddOrder(true);
    }

    return (
        <Container component="main" maxWidth="lg" sx={{marginTop: '50px'}}>
            <Container component="div" sx={{marginLeft: '-20px', marginBottom: '20px', display: 'flex'}}>
                <Container component="div">
                    <Button variant="contained" sx={{marginLeft: '-20px'}} onClick={handleSortOpen}>Sort</Button>
                    <Button variant="contained" sx={{marginLeft: '20px'}} onClick={handleFilterOpen}>Filter</Button>
                </Container>
                {(user.role.roleType === 'Admin' || user.role.roleType === 'Editor') ? (
                    <Button variant="contained" color="success" sx={{marginRight: '-30px'}}
                            onClick={handleAddOrderOpen}>
                        Add
                    </Button>
                ) : (
                    <></>
                )}
            </Container>
            {/* Sort Dialog */}
            <Dialog open={openSort} onClose={handleSortClose}>
                <DialogTitle>Sort</DialogTitle>
                <DialogContent>
                    <DialogContentText>
                        Choose any sorting criteria and type from the following:
                    </DialogContentText>
                    <FormControl fullWidth>
                        <Select name="sort-criteria" sx={{marginTop: '10px'}} onChange={handleSortCriteriaChange}
                                defaultValue="none" value={sortCriteria}>
                            <MenuItem value="none">None</MenuItem>
                            <MenuItem value="payment">Payment</MenuItem>
                            <MenuItem value="shipmentDate">Shipment Date</MenuItem>
                            <MenuItem value="arrivalDate">Arrival Date</MenuItem>
                        </Select>
                    </FormControl>
                    <FormControl fullWidth>
                        <Select name="sort-type" sx={{marginTop: '10px'}} onChange={handleSortTypeChange}
                                defaultValue="asc" value={sortType}>
                            <MenuItem value="asc">Ascending</MenuItem>
                            <MenuItem value="desc">Descending</MenuItem>
                        </Select>
                    </FormControl>
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleSortClose}>Close</Button>
                </DialogActions>
            </Dialog>
            {/* Filter dialog */}
            <Dialog open={openFilter} onClose={handleFilterClose}>
                <DialogTitle>Filter</DialogTitle>
                <DialogContent>
                    <DialogContentText>
                        Choose any filter criteria and value from the following:
                    </DialogContentText>
                    <FormControl fullWidth>
                        <Select name="filter-sending-country"
                                sx={{marginTop: '10px'}}
                                onChange={handleSendingCountryChange}
                                defaultValue={0} value={filterSendingCountry}>
                            <MenuItem value={0}>Sending Country</MenuItem>
                            {countries && countries.map(country => (
                                <MenuItem key={country.countryId}
                                          value={country.countryId}
                                >
                                    {country.countryName}
                                </MenuItem>
                            ))}
                        </Select>
                    </FormControl>
                    <FormControl fullWidth>
                        <Select name="filter-destination-country" sx={{marginTop: '10px'}}
                                onChange={handleDestinationCountryChange}
                                defaultValue={0} value={filterDestinationCountry}>
                            <MenuItem value={0}>Destination Country</MenuItem>
                            {countries && countries.map(country => (
                                <MenuItem key={country.countryId}
                                          value={country.countryId}
                                >
                                    {country.countryName}
                                </MenuItem>
                            ))}
                        </Select>
                    </FormControl>
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleFilterClose}>Close</Button>
                    <Button onClick={handleFilterClear}>Clear Filter</Button>
                </DialogActions>
            </Dialog>
            {/*Add order dialog*/}
            <AddOrder openAddOrder={openAddOrder} handleAddOrderClose={handleAddOrderClose}/>
            <TableContainer component={Paper}>
                <Table sx={{minWidth: 650}} aria-label="simple table">
                    <TableHead>
                        <TableRow>
                            <TableCell>User</TableCell>
                            <TableCell align="right">Shipment Date</TableCell>
                            <TableCell align="right">Arrival Date</TableCell>
                            <TableCell align="right">Cargo Type</TableCell>
                            <TableCell align="right">Payment</TableCell>
                            <TableCell align="right">Payment Method</TableCell>
                            <TableCell align="right">Sending Country</TableCell>
                            <TableCell align="right">Destination Country</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {rows.map((row) => (
                            <TableRow
                                key={row.id}
                                sx={{'&:last-child td, &:last-child th': {border: 0}}}
                            >
                                <TableCell component="th" scope="row">
                                    {row.userName}
                                </TableCell>
                                <TableCell align="right">{row.shipmentDate.toLocaleDateString()}</TableCell>
                                <TableCell align="right">{row.arrivalDate.toLocaleDateString()}</TableCell>
                                <TableCell align="right">{row.cargoType}</TableCell>
                                <TableCell align="right">{row.payment}</TableCell>
                                <TableCell align="right">{row.paymentMethod}</TableCell>
                                <TableCell align="right">{row.sendingCountry}</TableCell>
                                <TableCell align="right">{row.destinationCountry}</TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>
            <TablePagination
                rowsPerPageOptions={[5, 10]}
                component="div"
                count={count}
                rowsPerPage={rowsPerPage}
                page={page}
                onPageChange={handlePageChange}
                onRowsPerPageChange={handleRowsPerPageChange}
            />
        </Container>
    );
}