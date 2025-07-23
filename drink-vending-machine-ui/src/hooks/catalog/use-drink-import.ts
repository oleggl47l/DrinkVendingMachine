import {useRef, useState} from "react";
import {DrinkService} from "@/app/api/drink-vending-machine";
import {useToast} from "@/components/ui/toast";

export const useDrinkImport = (onSuccess?: () => void) => {
    const fileInputRef = useRef<HTMLInputElement>(null);
    const [uploading, setUploading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const {showToast} = useToast();

    const handleImport = async (file: File | null) => {
        if (!file) return;

        setUploading(true);
        setError(null);

        try {
            await DrinkService.importDrinksFromExcel({formData: {file}});

            onSuccess?.();
            fileInputRef.current!.value = "";
            showToast("Импорт успешно завершён", "success");
        } catch (e) {
            console.error("Ошибка при импорте:", e);
            setError("Не удалось импортировать файл");
            showToast("Не удалось импортировать файл", "error");
        } finally {
            setUploading(false);
        }
    };

    return {
        uploading,
        error,
        fileInputRef,
        handleImport,
    };
};
