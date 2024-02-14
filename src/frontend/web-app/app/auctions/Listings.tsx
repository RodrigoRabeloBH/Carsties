'use client'

import React, { useEffect, useState } from 'react'
import AuctionCard from './AuctionCard';
import { Auction, PageResult } from '@/types';
import AppPagination from '../components/AppPagination';
import { getData } from '../actions/auctionActions';
import Filters from './Filters';
import { useParamsStore } from '@/hooks/useParamsStore';
import { shallow } from 'zustand/shallow';
import EmptyFilter from '../components/EmptyFilter';

export default function Listings() {

    const [data, setData] = useState<PageResult<Auction>>();

    const params = useParamsStore(state => ({
        pageNumber: state.pageNumber,
        pageSize: state.pageSize,
        searchTerm: state.searchTerm,
        orderBy: state.orderBy,
        filterBy: state.filterBy,
        seller: state.seller,
        winner: state.winner,
    }), shallow);

    const setParams = useParamsStore(state => state.setParams);

    function setPageNumber(pageNumber: number) {
        setParams({ pageNumber })
    }

    useEffect(() => {
        getData(params.pageNumber, params.pageSize,
            params.searchTerm, params.orderBy, params.filterBy,
            params.seller, params.winner)
            .then(data => {
                setData(data);
            })
    }, [params.pageNumber, params.pageSize, params.searchTerm, params.orderBy, params.filterBy, params.seller, params.winner]);

    if (!data)
        return <h3>Loading ...</h3>

    return (
        <>
            <Filters />
            {data.totalCount === 0 ? (
                <EmptyFilter showRest />
            ) : (
                <>
                    <div className='grid grid-cols-4 gap-6'>
                        {data.results.map((auction) => (
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
