import { useParamsStore } from '@/hooks/useParamsStore'
import React from 'react'
import Heading from './Heading'
import { Button } from 'flowbite-react'

type Props = {
    title?: string
    subtitle?: string
    showRest?: boolean
}
export default function EmptyFilter({
    subtitle = 'Try changing or resetting the filter',
    title = 'No matches for this filter',
    showRest
}: Props) {
    const reset = useParamsStore(state => state.reset);
    return (
        <div className='h-[40vh] flex flex-col gap-2 justify-center items-center shadow-lg'>
            <Heading subtitle={subtitle} title={title} center />
            <div className='mt-4'>
                {showRest && (
                    <Button outline onClick={reset}>Remove Filters</Button>
                )}
            </div>
        </div>
    )
}
