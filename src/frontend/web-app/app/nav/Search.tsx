'use client'
import { useParamsStore } from '@/hooks/useParamsStore'
import { usePathname, useRouter } from 'next/navigation';
import React from 'react'
import { FaSearch } from 'react-icons/fa'

export default function Search() {
    const setParams = useParamsStore(state => state.setParams);
    const setSearchValue = useParamsStore(state => state.setSearchValue);
    const searchValue = useParamsStore(state => state.searchValue);
    const router = useRouter();
    const pathname = usePathname();

    function onChange(event: any) {
        setSearchValue(event.target.value);
    }

    function search() {
        if (pathname !== '/')
            router.push('/');
        setParams({ searchTerm: searchValue });
    }

    return (
        <div className='flex lg:w-[40%] items-center border-2 rounded-full py-2 shadow-sm m-2'>
            <input
                onKeyDown={(e: any) => {
                    if (e.key === 'Enter')
                        search();
                }}
                value={searchValue}
                onChange={onChange}
                type="text"
                placeholder='Search ...'
                className='text-sm  text-gray-600 input-custom'
            />
            <button onClick={search}>
                <FaSearch size={34} className='bg-red-400 text-white rounded-full p-2 cursor-pointer mx-2' />
            </button>
        </div>
    )
}
