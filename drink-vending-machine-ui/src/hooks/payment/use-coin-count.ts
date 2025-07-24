import {useState, useEffect} from 'react';

export function useCoinCount(
    count: number,
    onChange: (delta: number) => void
) {
    const [inputValue, setInputValue] = useState(String(count));

    useEffect(() => {
        setInputValue(String(count));
    }, [count]);

    const handleInputChange = (value: string) => {
        if (/^\d*$/.test(value)) {
            setInputValue(value);
            const parsed = parseInt(value, 10);
            const newCount = isNaN(parsed) ? 0 : parsed;
            const delta = newCount - count;
            if (delta !== 0) onChange(delta);
        }
    };

    const handleBlur = () => {
        const parsed = parseInt(inputValue, 10);
        const safe = isNaN(parsed) ? 0 : parsed;
        const delta = safe - count;
        if (delta !== 0) onChange(delta);
        setInputValue(String(safe));
    };

    const increment = () => onChange(1);
    const decrement = () => {
        if (count > 0) onChange(-1);
    };

    return {
        inputValue,
        handleInputChange,
        handleBlur,
        increment,
        decrement,
    };
}
