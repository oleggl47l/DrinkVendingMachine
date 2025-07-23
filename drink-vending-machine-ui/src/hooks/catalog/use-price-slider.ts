import {useState, useRef, useEffect} from "react";

export const usePriceSlider = (initialValue: number) => {
    const [currentValue, setCurrentValue] = useState(initialValue);
    const [showTooltip, setShowTooltip] = useState(false);
    const [tooltipPosition, setTooltipPosition] = useState(0);
    const sliderRef = useRef<HTMLInputElement>(null);

    useEffect(() => {
        setCurrentValue(initialValue);
    }, [initialValue]);

    const updateTooltip = (value: number) => {
        if (!sliderRef.current) return;
        const min = Number(sliderRef.current.min);
        const max = Number(sliderRef.current.max);
        setTooltipPosition(((value - min) / (max - min)) * 100);
    };

    const handleChange = (value: number) => {
        setCurrentValue(value);
        updateTooltip(value);
        setShowTooltip(true);
    };

    const hideTooltip = () => setShowTooltip(false);
    const showTooltipOnHover = () => {
        setShowTooltip(true);
        updateTooltip(currentValue);
    };

    return {
        currentValue,
        setCurrentValue,
        showTooltip,
        tooltipPosition,
        sliderRef,
        handleChange,
        hideTooltip,
        showTooltipOnHover,
    };
};
