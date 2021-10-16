import React, { Component } from 'react';
import './Cars.scss';

import CarBlock from '../../components/CarBlock/CarBlock';
import CarFilter from '../../components/Filter/CarFilter';

export class Cars extends Component {
    static displayName = Cars.name;

    render() {
        return (
            <div className="cars-section">
                <div className="cars">
                    <CarBlock />
                    <CarBlock />
                </div>

                <div className="filter">
                    <CarFilter />
                </div>
            </div>
        );
    }
}
