'use client'

import { placeBidForAuction } from '@/app/actions/auctionActions';
import { numberWithCommas } from '@/app/lib/numberWithCommas';
import { useBidStore } from '@/hooks/useBidStore';
import { Button } from 'flowbite-react';
import React from 'react'
import { FieldValues, useForm } from 'react-hook-form';
import toast from 'react-hot-toast';

type Props = {
    auctionId: string;
    highBid: number;
}

export default function BidForm({ auctionId, highBid }: Props) {
    const { register, handleSubmit, reset, formState: { errors, isSubmitting } } = useForm();
    const addBid = useBidStore(state => state.addBid);

    function onSubmit(data: FieldValues) {
        placeBidForAuction(auctionId, +data.amount)
            .then(bid => {
                if (bid.error) throw bid.error;
                addBid(bid);
                toast.success('Bid added successfully');
            }).catch(error => {              
                toast.error(error.message);
            }).finally(() => {
                reset();
            });
    }

    return (
        <form onSubmit={handleSubmit(onSubmit)} className='flex items-center border-2 rounded-lg py-2'>
            <input
                type="number"
                {...register('amount')}
                className='input-custom text-sm text-gray-600'
                placeholder={`Enter your bid (minimum bid is $${numberWithCommas(highBid + 1)})`}
            />
            <div className='flex justify-between px-2'>
                <Button
                    size='sm'
                    gradientDuoTone='greenToBlue'
                    isProcessing={isSubmitting}
                    type='submit'
                    color='success'>
                    Submit
                </Button>
            </div>
        </form>
    )
}
