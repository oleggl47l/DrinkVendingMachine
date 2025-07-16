import {useState, useEffect} from 'react';
import {BrandModel, BrandService, DrinkModel, DrinkService} from "@/app/api/drink-vending-machine";
import {debounce} from "next/dist/server/utils";

export const useDrinks = () => {
    const [drinks, setDrinks] = useState<DrinkModel[]>([]);
    const [brands, setBrands] = useState<BrandModel[]>([]);
    const [selectedBrand, setSelectedBrand] = useState<number | null>(null);
    const [priceBounds, setPriceBounds] = useState<[number, number]>([0, 1000]);
    const [selectedRange, setSelectedRange] = useState<[number, number]>([0, 1000]);
    const [loading, setLoading] = useState(true);
    const [selectedDrinkIds, setSelectedDrinkIds] = useState<Set<number>>(new Set());

    const loadInitialData = async () => {
        try {
            const [brandsData, priceRangeData, drinksData] = await Promise.all([
                BrandService.getAllBrands(),
                DrinkService.getDrinksPriceRange({}),
                DrinkService.getAllDrinks({})
            ]);

            const bounds: [number, number] = [priceRangeData.minPrice || 0, priceRangeData.maxPrice || 1000];
            setPriceBounds(bounds);
            setSelectedRange(bounds);
            setBrands(brandsData);
            setDrinks(drinksData);
        } catch (error) {
            console.error('Error loading initial data:', error);
        } finally {
            setLoading(false);
        }
    };

    const handleBrandChange = async (brandId: number | null) => {
        setSelectedBrand(brandId);
        setLoading(true);

        try {
            const newPriceRange = await DrinkService.getDrinksPriceRange({brandId: brandId || undefined});

            const bounds: [number, number] = [
                newPriceRange.minPrice || 0,
                newPriceRange.maxPrice || (newPriceRange.minPrice || 0) + 1000
            ];

            setPriceBounds(bounds);
            setSelectedRange(bounds);

            const filteredDrinks = await DrinkService.getAllDrinks({
                brandId: brandId || undefined,
                minPrice: bounds[0],
                maxPrice: bounds[1]
            });

            setDrinks(filteredDrinks);
        } catch (error) {
            console.error('Error filtering drinks:', error);
        } finally {
            setLoading(false);
        }
    };

    const toggleSelectDrink = (id: number) => {
        setSelectedDrinkIds((prev) => {
            const newSet = new Set(prev);
            if (newSet.has(id)) {
                newSet.delete(id);
            } else {
                newSet.add(id);
            }
            return newSet;
        });
    };

    const handlePriceChange = debounce(async (newRange: [number, number]) => {
        setSelectedRange(newRange);
        setLoading(true);

        try {
            const filteredDrinks = await DrinkService.getAllDrinks({
                brandId: selectedBrand || undefined,
                minPrice: newRange[0],
                maxPrice: newRange[1]
            });
            setDrinks(filteredDrinks);
        } catch (error) {
            console.error('Error filtering drinks by price:', error);
        } finally {
            setLoading(false);
        }
    }, 300);

    useEffect(() => {
        const fetchData = async () => {
            try {
                await loadInitialData();
            } catch (err) {
                console.error('Unhandled error during fetchData in useEffect', err);
            }
        };

        void fetchData();
    }, []);


    return {
        drinks,
        brands,
        selectedBrand,
        priceBounds,
        selectedRange,
        loading,
        handleBrandChange,
        handlePriceChange,
        selectedDrinkIds,
        toggleSelectDrink
    };
};