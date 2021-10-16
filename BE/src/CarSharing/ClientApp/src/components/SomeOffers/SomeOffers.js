import React, { Component } from 'react';
import './SomeOffers.scss';
import Vector from '../Images/Vector.png';
import "slick-carousel/slick/slick.css";
import "slick-carousel/slick/slick-theme.css";
import { Link } from "react-router-dom";

export default class SomeOffers extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            data: []
        };
    }

    componentDidMount() {
        fetch("api/getHomePageCars")
            .then((res) => res.json())
            .then(
                (data) => {
                    this.setState({
                        data: data
                    });
                },
                (error) => {
                    console.log(error)
                }
            );
    }

    render() {
        const { data } = this.state;
        return (
            <section className="some-offers">
                <h2> Some offers </h2>

                <div className="offers-container">
                    {data.map(car => 
                        <div className="car-container">
                            <div className="image-container">
                                <img src={car.carImage} alt={car.title} />
                            </div>
                            <p className="car-title">{car.title}</p>
                            <div className="caracteristics">
                                <div >
                                    <p>{car.transmission}</p>
                                    <p>{car.kilometers} km</p>
                                </div>
                                <div>
                                    <p>{car.color}</p>
                                    <p>{car.seats} seats</p>
                                </div>
                            </div>
                            <p className="price">
                                {car.pricePerDay} $
                            </p>
                            <Link
                                to={{
                                    pathname: `/details/${car.id}`,
                                }}
                            >
                                <div className="price-button">
                                    <button type="button">Reserve now <img src={Vector} alt="right arrow" /></button>
                                </div>
                            </Link>
                            
                        </div>
                    )}
                </div>
            </section>
        );
    }
}