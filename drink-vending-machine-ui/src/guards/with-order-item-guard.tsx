'use client';

import React from 'react';
import {useOrderContext} from '@/context/order-context';
import {useRouter} from 'next/navigation';

export function withOrderItemGuard<P extends object>(
    WrappedComponent: React.ComponentType<P>
) {
    return function GuardedComponent(props: P) {
        const {orderItems, isHydrated} = useOrderContext();
        const router = useRouter();

        React.useEffect(() => {
            if (!isHydrated) return;
            if (orderItems.length === 0) {
                router.replace('/catalog');
            }
        }, [isHydrated, orderItems, router]);

        if (!isHydrated || orderItems.length === 0) {
            return null;
        }

        return <WrappedComponent {...props} />;
    };
}