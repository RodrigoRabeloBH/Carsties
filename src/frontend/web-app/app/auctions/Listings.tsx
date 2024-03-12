'use client'

import React, { useEffect, useState } from 'react'
import AuctionCard from './AuctionCard';
import AppPagination from '../components/AppPagination';
import { getData } from '../actions/auctionActions';
import Filters from './Filters';
import { useParamsStore } from '@/hooks/useParamsStore';
import { shallow } from 'zustand/shallow';
import EmptyFilter from '../components/EmptyFilter';
import { useAuctionStore } from '@/hooks/useAuctionStore';

export default function Listings() {
    const [loading, setLoading] = useState(true);

    const params = useParamsStore(state => ({
        pageNumber: state.pageNumber,
        pageSize: state.pageSize,
        searchTerm: state.searchTerm,
        orderBy: state.orderBy,
        filterBy: state.filterBy,
        seller: state.seller,
        winner: state.winner,
    }), shallow);

    const data = useAuctionStore(state => ({
        auctions: state.auctions,
        totalCount: state.totalCount,
        pageCount: state.pageCount
    }), shallow);

    const setData = useAuctionStore(state => state.setData);
    const setParams = useParamsStore(state => state.setParams);

    function setPageNumber(pageNumber: number) {
        setParams({ pageNumber })
    }

    useEffect(() => {
        getData(params.pageNumber, params.pageSize,
            params.searchTerm, params.orderBy, params.filterBy,
            params.seller, params.winner)
            .then(response => {
                setData(response);
                setLoading(false);
            })
    }, [params.pageNumber, params.pageSize, params.searchTerm, params.orderBy, params.filterBy, params.seller, params.winner, setData]);

    if (loading)
        return <h3>Loading ...</h3>

    return (
        <>
            <Filters />
            {data.totalCount === 0 ? (
                <EmptyFilter showRest />
            ) : (
                <>
                    <div className='grid md:grid-cols-2 lg:grid-cols-4 gap-6'>
                        {data.auctions.map((auction) => (
                            <AuctionCard auction={auction} key={auction.id} />
                        ))}
                    </div>
                    <div className='flex justify-center mt-4'>
                        <AppPagination pageChanged={setPageNumber} currentPage={params.pageNumber} pageCount={data.pageCount} />
                    </div>
                </>
            )}
        </>
    )
}
