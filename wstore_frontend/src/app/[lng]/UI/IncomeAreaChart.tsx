"use client"
import React, {useState} from 'react';
import {
    AreaChart,
    Area,
    XAxis,
    YAxis,
    CartesianGrid,
    ResponsiveContainer,
} from 'recharts';
import { statisticsApi } from "@/services/statisticsService";
import CustomSelect from "@/app/[lng]/UI/SelectButton";

const formatDate = (isoString: string) => {
    const d = new Date(isoString);
    return `${String(d.getDate()).padStart(2, '0')}.${String(d.getMonth() + 1).padStart(2, '0')}`;
};

interface CustomDotProps {
    cx?: number;
    cy?: number;
    payload?: {
        interval: number;
        value: number;
        dateLabel: string;
    };
}

// Кастомна точка: кружок кольору --price + підпис координат над нею
const CustomDot = (props: CustomDotProps) => {
    const { cx, cy, payload } = props;
    if (!cy) return null;
    return (
        <g>
            <circle cx={cx} cy={cy} r={4} fill="var(--price)" stroke="var(--surface)" strokeWidth={1.5} />
            <text
                x={cx}
                y={cy - 14}
                textAnchor="middle"
                fontSize={13}
                fontWeight={600}
                fill="var(--text)"
            >
                {payload?.value}
            </text>
        </g>
    );
};

export default function IncomeAreaChart() {
    const times = ["30", "60", "90", "180", "365"];
    const [period, setPeriod] = useState<string | null>("30")
    const { data: rawData, isLoading, isError } = statisticsApi.useGetStatisticsQuery(Number(period));

    if (isLoading) {
        return (
            <div className="w-full rounded-xl p-5 border-2" style={{ height: 460, background: 'var(--surface)', borderColor: 'var(--border)', color: 'var(--text)' }}>
                Завантаження...
            </div>
        );
    }

    if (isError || !rawData) {
        return (
            <div className="w-full rounded-xl p-5 border-2" style={{ height: 460, background: 'var(--surface)', borderColor: 'var(--border)', color: 'var(--sale)' }}>
                Не вдалося завантажити дані
            </div>
        );
    }

    // interval = порядковий номер точки (1..n), value = дохід
    const chartData = rawData.map((item, i) => ({
        interval: i + 1,
        value: item.income,
        dateLabel: formatDate(item.startDate),
    }));

    return (
        <div
            className="w-full rounded-xl p-5 border-2"
            style={{ height: 460, background: 'var(--surface)', borderColor: 'var(--border)' }}
        >
            <div className="flex flex-row w-full justify-between">
            <div className="mb-2 text-lg font-semibold" style={{ color: 'var(--text)' }}>
                Прибуток за час
            </div>
            <CustomSelect name={"Period"} options={times} value={period} onChange={setPeriod} />
            </div>
            <ResponsiveContainer width="100%" height="85%">
                <AreaChart
                    data={chartData}
                    margin={{ top: 40, right: 30, left: 10, bottom: 50 }}
                >
                    <CartesianGrid stroke="var(--border)" />
                    <XAxis
                        dataKey="dateLabel"
                        type="category"
                        stroke="var(--muted)"
                        tick={{ fontSize: 13, angle: -90, textAnchor: 'end', fill: 'var(--muted)' }}
                        height={70}
                        interval={0}
                    />
                    <YAxis
                        stroke="var(--muted)"
                        tick={{ fontSize: 13, fill: 'var(--muted)' }}
                        domain={[0, 'dataMax + 500']}
                    />
                    <Area
                        type="linear"
                        dataKey="value"
                        stroke="var(--price)"
                        strokeWidth={2.5}
                        fill="var(--price)"
                        fillOpacity={0.25}
                        dot={<CustomDot />}
                        activeDot={false}
                        isAnimationActive={false}
                    />
                </AreaChart>
            </ResponsiveContainer>
        </div>
    );
}