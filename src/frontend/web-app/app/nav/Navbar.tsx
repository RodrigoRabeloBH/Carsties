import React from 'react'
import Search from './Search'
import Logo from './Logo';
import LogginButton from './LogginButton';
import { getCurrentUser } from '../actions/authActions';
import UserActions from './UserActions';

export default async function Navbar() {
    const user = await getCurrentUser();
    return (
        <header className='sticky top-0 z-50 lg:flex justify-between bg-white p-5 items-center text-gray-800 shadow-md md:grid grid-cols-1'>
            <Logo />
            <Search />
            {user ? (
                <UserActions user={user} />
            ) : (
                <LogginButton />
            )}
        </header>
    )
}
