'use client';

import React, { useEffect } from 'react';
import { useOrderContext } from '@/context/order-context';
import { useRouter } from 'next/navigation';
import type { JSX } from 'react';

export function withOrderItemGuard<P extends JSX.IntrinsicAttributes>(
    WrappedComponent: React.ComponentType<P>
) {
    return function GuardedComponent(props: P) {
        const { orderItems, isHydrated } = useOrderContext();
        const router = useRouter();

        useEffect(() => {
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