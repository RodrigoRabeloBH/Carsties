'use client'
import { useParamsStore } from '@/hooks/useParamsStore';
import { usePathname, useRouter } from 'next/navigation';
import React from 'react'
import { AiOutlineCar } from 'react-icons/ai'

export default function Logo() {
    const reset = useParamsStore(state => state.reset);
    const router = useRouter();
    const pathName = usePathname();

    function doReset() {
        if (pathName !== '/')
            router.push('/');
        reset();
    }

    return (
        <div className='flex items-center gap-2 md:text-1xl lg:text-3xl font-semibold text-red-500 m-2'>
            <AiOutlineCar size={34} />
            <div onClick={doReset} className='cursor-pointer'>
                Carsties Auctions
            </div>
        </div>
    )
}
