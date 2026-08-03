import { useFormikContext } from "formik";
import Icon from "@/app/[lng]/UI/Icon";

interface FormikNumberInputProps<T> {
    name: keyof T;
    label: string;
    type?: string;
    placeholder?: string;
    min?: number;
    max?: number;
    step?: number;
}

function FormikNumberInput<T extends Record<string, string | number>>({
                                                                          name,
                                                                          label,
                                                                          min = 1,
                                                                          max = Infinity,
                                                                          step = 1,
                                                                      }: FormikNumberInputProps<T>) {
    const { values, setFieldValue } = useFormikContext<T>();

    const currentValue = Number(values[name]) || 0;

    const clamp = (value: number) => Math.min(max, Math.max(min, value));

    const decrement = () => {
        setFieldValue(name as string, clamp(currentValue - step));
    };

    const increment = () => {
        setFieldValue(name as string, clamp(currentValue + step));
    };

    const isDecrementDisabled = currentValue <= min;
    const isIncrementDisabled = currentValue >= max;

    return (
        <div className="flex flex-row items-center gap-4" role="group" aria-label={label}>
            <Icon height={24} width={24}>
                <button
                    type="button"
                    onClick={decrement}
                    disabled={isDecrementDisabled}
                    aria-label={`Зменшити ${label}`}
                    className="disabled:opacity-40 disabled:cursor-not-allowed"
                >
                    -
                </button>
            </Icon>

            <span aria-live="polite">{currentValue}</span>

            <Icon height={24} width={24}>
                <button
                    type="button"
                    onClick={increment}
                    disabled={isIncrementDisabled}
                    aria-label={`Збільшити ${label}`}
                    className="disabled:opacity-40 disabled:cursor-not-allowed"
                >
                    +
                </button>
            </Icon>
        </div>
    );
}

export default FormikNumberInput;