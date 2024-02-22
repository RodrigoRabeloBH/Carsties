'use client'
import React, { useState } from 'react'
import { Button } from 'flowbite-react';
import { updateAuction } from '../actions/auctionActions';

export default function AuthTest() {
    const [loading, setLoading] = useState(false);
    const [result, setResult] = useState<any>();
    const id: string = '';

    function doUpdate() {
        setResult(undefined);
        setLoading(true);
        updateAuction({}, id)
            .then(res => setResult(res))
            .finally(() => setLoading(false));
    }
    return (
        <div className='flex items-center gap-4'>
            <Button outline isProcessing={loading} onClick={doUpdate}>
                Test Update
            </Button>
            <div>
                {JSON.stringify(result, null, 2)}
            </div>
        </div>
    )
}
