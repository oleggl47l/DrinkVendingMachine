'use client';

import { useVendingLock } from '@/hooks/use-vending-lock';
import { LockedScreen } from '@/components/ui/locked-screen';
import { ReactNode } from 'react';

interface Props {
    children: ReactNode;
}

export function VendingLockGuard({ children }: Props) {
    const { isLocked, refreshLock } = useVendingLock();

    if (isLocked) {
        return <LockedScreen onRefreshAAction={refreshLock} />;
    }

    return <>{children}</>;
}
