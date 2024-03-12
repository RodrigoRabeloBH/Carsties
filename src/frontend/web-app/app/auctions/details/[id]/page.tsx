import { getDetailedViewData } from '@/app/actions/auctionActions'
import Heading from '@/app/components/Heading';
import React from 'react'
import CountdownTimer from '../../CountdownTimer';
import CarImage from '../../CarImage';
import DetailedSpecs from './DetailedSpec';
import { getCurrentUser } from '@/app/actions/authActions';
import EditButton from './EditButton';
import DeleteButton from './DeleteButton';
import BidList from './BidList';

export default async function Details({ params }: { params: { id: string } }) {
    const auction = await getDetailedViewData(params.id);
    const user = await getCurrentUser();
    return (
        <div>
            <div className="sm:grid grid-cols-2">
                <div className='flex items-center justify-between gap-3 mb-2'>
                    <Heading title={`${auction.make} ${auction.model}`} />
                    {user?.username === auction.seller && (
                        <div className='flex gap-4 mr-3'>
                            <EditButton id={auction.id} />
                            <DeleteButton id={auction.id} />
                        </div>
                    )}
                </div>
                <div className="flex gap-3 items-center justify-end">
                    <h3 className='lg:text-2xl font-semibold'>
                        Time remaining
                    </h3>
                    <CountdownTimer auctionEnd={auction.auctionEnd} />
                </div>
            </div>
            <div className="lg:grid grid-cols-2 gap-6 mt-3">
                <div className="w-full bg-gray-200 aspect-h-10 aspect-w-16 rounded-lg overflow-hidden">
                    <CarImage imageUrl={auction.imageUrl} />
                </div>
                <BidList auction={auction} user={user} />
            </div>
            <div className="mt-3 grid grid-cols-1 rounded-lg">
                <DetailedSpecs auction={auction} />
            </div>
        </div>
    )
}
