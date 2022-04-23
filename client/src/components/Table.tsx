import * as React from "react";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import Paper from "@mui/material/Paper";

function createData(
    user: string,
    shipmentDate: Date,
    arrivalDate: Date,
    cargoType: string,
    payment: number,
    paymentMethod: string,
    sendingCountry: string,
    destinationCountry: string
) {
    return {
        user,
        shipmentDate,
        arrivalDate,
        cargoType,
        payment,
        paymentMethod,
        sendingCountry,
        destinationCountry,
    };
}

const rows = [
    createData(
        "John Doe",
        new Date(),
        new Date(),
        "Refrigerators",
        1000,
        "Transfer",
        "Romania",
        "Moldova"
    ),
    createData(
        "John Doe",
        new Date(),
        new Date(),
        "Refrigerators",
        1000,
        "Transfer",
        "Romania",
        "Moldova"
    ),
    createData(
        "John Doe",
        new Date(),
        new Date(),
        "Refrigerators",
        1000,
        "Transfer",
        "Romania",
        "Moldova"
    ),
    createData(
        "John Doe",
        new Date(),
        new Date(),
        "Refrigerators",
        1000,
        "Transfer",
        "Romania",
        "Moldova"
    ),
];

export default function DataTable() {
    return (
        <TableContainer sx={{ mt: 10 }} component={Paper}>
            <Table sx={{ minWidth: 650 }} aria-label="simple table">
                <TableHead>
                    <TableRow>
                        <TableCell align="left">User</TableCell>
                        <TableCell align="left">Shipment Date</TableCell>
                        <TableCell align="left">Arrival Date</TableCell>
                        <TableCell align="left">Cargo Type</TableCell>
                        <TableCell align="left">Payment</TableCell>
                        <TableCell align="left">Payment Method</TableCell>
                        <TableCell align="left">Sending Country</TableCell>
                        <TableCell align="left">Destination Country</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {rows.map((row) => (
                        <TableRow
                            key={row.user}
                            sx={{
                                "&:last-child td, &:last-child th": {
                                    border: 0,
                                },
                            }}
                        >
                            <TableCell component="th" scope="row">
                                {row.user}
                            </TableCell>
                            <TableCell align="left">
                                {row.shipmentDate.toLocaleDateString()}
                            </TableCell>
                            <TableCell align="left">
                                {row.arrivalDate.toLocaleDateString()}
                            </TableCell>
                            <TableCell align="left">{row.cargoType}</TableCell>
                            <TableCell align="left">{row.payment}</TableCell>
                            <TableCell align="left">
                                {row.paymentMethod}
                            </TableCell>
                            <TableCell align="left">
                                {row.sendingCountry}
                            </TableCell>
                            <TableCell align="left">
                                {row.destinationCountry}
                            </TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </TableContainer>
    );
}
