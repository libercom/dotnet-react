import MenuItem from "@mui/material/MenuItem";
import Typography from "@mui/material/Typography";
import {Link} from "react-router-dom";
import Menu from "@mui/material/Menu";
import * as React from "react";
import {useAuth} from "../context/AuthContext";

interface UserMenuParams {
    anchorElUser: HTMLElement | null;
    handleCloseUserMenu: () => void;
}

export const UserMenu = (params: UserMenuParams) => {
    const {anchorElUser, handleCloseUserMenu} = params;
    const {authenticated, logout} = useAuth();

    return (
        <Menu
            sx={{mt: '45px'}}
            id="menu-appbar"
            anchorEl={anchorElUser}
            anchorOrigin={{
                vertical: 'top',
                horizontal: 'right',
            }}
            keepMounted
            transformOrigin={{
                vertical: 'top',
                horizontal: 'right',
            }}
            open={Boolean(anchorElUser)}
            onClose={handleCloseUserMenu}
        >
            {authenticated ? [
                    <MenuItem key={1} onClick={handleCloseUserMenu}>
                        <Typography textAlign="center" sx={{textDecoration: 'none', color: 'black'}}
                                    component={Link}
                                    onClick={() => logout()}
                                    to='login'>Log out</Typography>
                    </MenuItem>
                ] :
                [
                    <MenuItem key={1} onClick={handleCloseUserMenu}>
                        <Typography textAlign="center" sx={{textDecoration: 'none', color: 'black'}}
                                    component={Link}
                                    to='login'>Log in</Typography>
                    </MenuItem>,
                    <MenuItem key={2} onClick={handleCloseUserMenu}>
                        <Typography textAlign="center" sx={{textDecoration: 'none', color: 'black'}}
                                    component={Link}
                                    to='register'>Register</Typography>
                    </MenuItem>
                ]
            }


        </Menu>
    )
}