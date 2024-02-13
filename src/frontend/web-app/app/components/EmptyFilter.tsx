'use client'
import { useParamsStore } from '@/hooks/useParamsStore'
import React from 'react'
import Heading from './Heading'
import { Button } from 'flowbite-react'
import { signIn } from 'next-auth/react'

type Props = {
    title?: string
    subtitle?: string
    showRest?: boolean
    showLogin?: boolean
    callbackUrl?: string
}
export default function EmptyFilter({
    subtitle = 'Try changing or resetting the filter',
    title = 'No matches for this filter',
    showRest,
    callbackUrl,
    showLogin
}: Props) {
    const reset = useParamsStore(state => state.reset);
    return (
        <div className='h-[40vh] flex flex-col gap-2 justify-center items-center shadow-lg'>
            <Heading subtitle={subtitle} title={title} center />
            <div className='mt-4'>
                {showRest && (
                    <Button outline onClick={reset}>Remove Filters</Button>
                )}
                {showLogin && (
                    <Button outline onClick={() => signIn('id-server', { callbackUrl })}>Login</Button>
                )}
            </div>
        </div>
    )
}
