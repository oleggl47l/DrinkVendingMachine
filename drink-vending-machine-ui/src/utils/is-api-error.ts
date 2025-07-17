import { ApiError } from "@/app/api/drink-vending-machine/core/ApiError";

type ProblemDetails = {
    type?: string;
    title?: string;
    status?: number;
    instance?: string;
    errorName?: string;
    args?: Array<{ name: string; value: unknown }>;
};

export function isApiError(error: unknown, expectedErrorName: string): error is ApiError {
    if (error instanceof ApiError) {
        const body = error.body as ProblemDetails;
        return body?.errorName === expectedErrorName;
    }
    return false;
}
