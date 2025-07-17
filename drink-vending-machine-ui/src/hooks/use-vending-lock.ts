import {useEffect, useState, useRef, useCallback} from 'react';
import {VendingService} from '@/app/api/drink-vending-machine';
import {getOrCreateClientId} from "@/app/utils/client-id";

export const useVendingLock = () => {
    const [lockedByOther, setLockedByOther] = useState(false);
    const clientId = useRef<string | null>(null);
    const heartbeatInterval = useRef<NodeJS.Timeout | null>(null);

    const startHeartbeat = useCallback(() => {
        if (heartbeatInterval.current) return;
        heartbeatInterval.current = setInterval(async () => {
            try {
                await VendingService.postApiV1VendingLockHeartbeat({
                    clientId: clientId.current || undefined
                });
            } catch {
                console.warn('Heartbeat error');
            }
        }, 15000);
    }, []);

    const checkLockStatus = useCallback(async () => {
        try {
            const status = await VendingService.getApiV1VendingLockStatus();
            if (status.locked && status.clientId !== clientId.current) {
                setLockedByOther(true);
            } else {
                setLockedByOther(false);
            }
            return !status.locked || status.clientId === clientId.current;
        } catch (e) {
            console.error('Ошибка при проверке статуса блокировки', e);
            return false;
        }
    }, []);

    const acquireLock = useCallback(async () => {
        clientId.current = getOrCreateClientId();
        try {
            const res = await VendingService.postApiV1VendingLockAcquire({
                clientId: clientId.current!
            });
            if (!res.acquired) {
                setLockedByOther(true);
            } else {
                startHeartbeat();
                setLockedByOther(false);
            }
        } catch (e) {
            console.error('Ошибка при попытке захвата автомата', e);
        }
    }, [startHeartbeat]);

    const releaseLock = useCallback(async () => {
        if (clientId.current) {
            try {
                await VendingService.postApiV1VendingLockRelease({
                    clientId: clientId.current
                });
            } catch (e) {
                console.error('Ошибка при освобождении блокировки', e);
            }
        }
        if (heartbeatInterval.current) {
            clearInterval(heartbeatInterval.current);
            heartbeatInterval.current = null;
        }
    }, []);

    const refreshLock = useCallback(async () => {
        const isAvailable = await checkLockStatus();
        if (isAvailable) {
            await acquireLock();
        }
    }, [checkLockStatus, acquireLock]);

    useEffect(() => {
        const init = async () => {
            await acquireLock();
        };

        void init();

        window.addEventListener('beforeunload', releaseLock);
        return () => {
            void releaseLock();
            window.removeEventListener('beforeunload', releaseLock);
        };
    }, [acquireLock, releaseLock]);


    return {
        isLocked: lockedByOther,
        refreshLock
    };
};