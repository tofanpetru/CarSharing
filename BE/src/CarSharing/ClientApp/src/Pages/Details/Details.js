import React, { Component } from 'react';
import './Details.scss';
import Footer from '../../components/Footer/Footer';
import SingleCar from '../../components/Singlecar/SingleCar';
import Services from '../../components/Services/Services';

export class Details extends Component {
    static displayName = Details.name;

    render() {
        return (
            <div className="details-section">
                <SingleCar />
                <Services/>

                <Footer />
            </div>
        );
    }
}
