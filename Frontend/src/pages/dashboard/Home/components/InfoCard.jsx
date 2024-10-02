import React, { useEffect, useState } from 'react';
import { Card, CardHeader, CardBody, Typography } from "@material-tailwind/react";

const InfoCard = ({ title, value, colorClass }) => {
  return (
    <Card className={`bg-surface-white mb-4 lg:w-3/4 lg:col-auto xl:mb-0 sm:w-3/5 sm:mx-auto md:mx-auto md:w-3/4 col-span-3 md:col-span-1 text-surface-black ${colorClass}`}>
      <CardHeader
        floated={false}
        shadow={false}
        color="transparent"
        className="m-0 p-6"
      >
        <Typography variant="h6" className="mb-2 text-surface-black">
          {title}
        </Typography>
      </CardHeader>
      <CardBody className="pt-0">
        <Typography variant="h3" className="text-surface-dark">
          {value}
        </Typography>
      </CardBody>
    </Card>
  );
};

export default InfoCard;
